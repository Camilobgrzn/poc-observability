using Facturacion.Consumers;
using MassTransit;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.OpenTelemetry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// Add masstransit services

builder.Services.AddMassTransit(x =>
{
    x.AddConsumers((typeof(ConsumidorGenerarFactura).Assembly));
    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "amqp://guest:guest@host.docker.internal:5672";
         cfg.Host(rabbitHost);
         cfg.ConfigureEndpoints(context);
    });
});

// Configure serilog

builder.Host.UseSerilog((context, configuration) =>
{
    var elasticUri = Environment.GetEnvironmentVariable("ELASTIC_URI") ?? "http://elastic:fenix123@host.docker.internal:9200";
    configuration
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
        {
            IndexFormat = "applogs-{0:yyyy.MM}",
            AutoRegisterTemplate = true
        }).WriteTo.OpenTelemetry(options =>
        {
            options.Endpoint = "http://host.docker.internal:4317";
            options.ResourceAttributes.Add("service.name", "Facturacion");
        })
        //.WriteTo.OpenTelemetry(
        //    endpoint: "http://host.docker.internal:4317",
        //    protocol:OtlpProtocol.HttpProtobuf)
        .Enrich.WithProperty("Application", "Facturacion");
});

// Configure OpenTelemetry
var resourceBuilder = ResourceBuilder.CreateDefault()
    .AddService("Facturacion");

builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSource("MassTransit")
            .SetResourceBuilder(resourceBuilder)
            .AddOtlpExporter(options =>
            {
                var elasticpApm = Environment.GetEnvironmentVariable("ELASTIC_APM_URI") ?? "http://host.docker.internal:8400";
                options.Endpoint = new Uri(elasticpApm);
            })
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri("http://host.docker.internal:4317");
            });

    })
    .WithMetrics(metricProviderBuilder =>
    {
        metricProviderBuilder
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddMeter("MassTransit")
            .AddMeter("CustomMetrics")
            .SetResourceBuilder(resourceBuilder)
            .AddOtlpExporter(options =>
            {
                var elasticApm = Environment.GetEnvironmentVariable("ELASTIC_APM_URI") ?? "http://host.docker.internal:8400";
                options.Endpoint = new Uri(elasticApm);
            })
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri("http://host.docker.internal:4317");
            });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();