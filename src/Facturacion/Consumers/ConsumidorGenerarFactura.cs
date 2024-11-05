using MassTransit;
using Mensajes.Facturacion;

namespace Facturacion.Consumers
{
    public class ConsumidorGenerarFactura : IConsumer<GenerarFactura>
    {
        private readonly ILogger<ConsumidorGenerarFactura> _logger;

        public ConsumidorGenerarFactura(ILogger<ConsumidorGenerarFactura> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<GenerarFactura> context)
        {
            var random = new Random();
            int delay = random.Next(0, 1000);
            await Task.Delay(delay);
            await context.Publish(new FacturaGenerada() { IdOrden = context.Message.IdOrden });
            _logger.LogInformation($"Factura generada para la orden: {context.Message.IdOrden}");
        }
    }
}
