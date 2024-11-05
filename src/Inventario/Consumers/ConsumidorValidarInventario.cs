using MassTransit;
using Mensajes.Inventario;

namespace Inventario.Consumers
{
    public class ConsumidorValidarInventario : IConsumer<ValidarInventario>
    {
        private readonly ILogger<ConsumidorValidarInventario> _logger;

        public ConsumidorValidarInventario(ILogger<ConsumidorValidarInventario> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ValidarInventario> context)
        {
            var random = new Random();
            int delay = random.Next(0, 1000);
            await Task.Delay(delay);
            await context.Publish(new InventarioValidado() { IdOrden = context.Message.IdOrden });
            _logger.LogInformation($"Inventario validado para la orden: {context.Message.IdOrden}");
        }
    }
}
