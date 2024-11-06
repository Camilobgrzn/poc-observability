using Inventario.Consumers;
using MassTransit;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// Add masstransit services

builder.Services.AddMassTransit(x =>
{
    x.AddConsumers(typeof(ConsumidorValidarInventario).Assembly);
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
    var elasticUri = Environment.GetEnvironmentVariable("ELASTIC_URI") ?? "http://localhost:9200";
    configuration
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
        {
            IndexFormat = "applogs-{0:yyyy.MM}",
            AutoRegisterTemplate = true
        })
        .Enrich.WithProperty("Application", "Inventario");
});

// Configure OpenTelemetry

var resourceBuilder = ResourceBuilder.CreateDefault()
    .AddService("Inventario");

builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .SetResourceBuilder(resourceBuilder)
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSource("MassTransit")
            .AddOtlpExporter(options =>
            {
                var elasticpApm = Environment.GetEnvironmentVariable("ELASTIC_APM_URI") ?? "http://localhost:8200";
                options.Endpoint = new Uri(elasticpApm);
            });
    })
    .WithMetrics(metricProviderBuilder =>
    {
        metricProviderBuilder
            .SetResourceBuilder(resourceBuilder)
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation()
            .AddMeter("MassTransit")
            .AddOtlpExporter(options =>
            {
                var elasticApm = Environment.GetEnvironmentVariable("ELASTIC_APM_URI") ?? "http://localhost:8200";
                options.Endpoint = new Uri(elasticApm);
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