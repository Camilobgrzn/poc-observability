using MassTransit;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Procesos;
using Serilog;
using Serilog.Sinks.Elasticsearch;

IHost host = Host.CreateDefaultBuilder(args)
    //Configure Serilog
    .UseSerilog((context, configuration) =>
    {
        var elasticUri = Environment.GetEnvironmentVariable("ELASTIC_URI") ?? "http://localhost:9200";
        configuration
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
            {
                IndexFormat = "applogs-{0:yyyy.MM}",
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
            })
            .Enrich.WithProperty("Application", "Procesos");
    })
    .ConfigureServices(services =>
    {
        services.AddMassTransit(x =>
        {
            x.AddSagaStateMachine<OrdenMachineState, OrdenState>().InMemoryRepository();
            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "amqp://guest:guest@host.docker.internal:5672";
                cfg.Host(rabbitHost);
                cfg.ConfigureEndpoints(context);
            });
        });

        // Configure OpenTelemetry
        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService("Procesos");

        services.AddOpenTelemetry()
            .WithTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                    .SetResourceBuilder(resourceBuilder)
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
                    .AddRuntimeInstrumentation()
                    .AddMeter("MassTransit")
                    .AddOtlpExporter(options =>
                    {
                        var elasticpApm = Environment.GetEnvironmentVariable("ELASTIC_APM_URI") ?? "http://localhost:8200";
                        options.Endpoint = new Uri(elasticpApm);
                    });
            });

    })
    .Build();

await host.RunAsync();
