
using MassTransit;
using Procesos;

IHost host = Host.CreateDefaultBuilder(args)
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

    })
    .Build();

await host.RunAsync();
