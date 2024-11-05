using MassTransit;
using Mensajes.Ordenes;

namespace Ordenes.Consumers
{
    public class ConsumidorCrearOrden : IConsumer<CrearOrden>
    {
        private readonly ILogger<ConsumidorCrearOrden> _logger;
        public ConsumidorCrearOrden(ILogger<ConsumidorCrearOrden> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CrearOrden> context)
        {
            var random = new Random();
            int delay = random.Next(0, 1000); 
            await Task.Delay(delay);
            await context.Publish(new OrdenCreada() { IdOrden = context.Message.Id });
            _logger.LogInformation($"Orden creada: {context.Message.Id}");
        }
    }
}
