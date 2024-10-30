
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
                cfg.Host("rabbitmq://localhost");
            });
        });

    })
    .Build();

await host.RunAsync();
