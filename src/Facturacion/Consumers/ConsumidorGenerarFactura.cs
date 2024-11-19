using MassTransit;
using Mensajes.Facturacion;
using System.Diagnostics.Metrics;

namespace Facturacion.Consumers
{
    public class ConsumidorGenerarFactura : IConsumer<GenerarFactura>
    {
        private readonly ILogger<ConsumidorGenerarFactura> _logger;
        private static readonly Meter MyMeter = new Meter("CustomMetrics", "1.0");
        private static readonly Counter<long> MyCustomCounter = MyMeter.CreateCounter<long>("my_custom_counter");

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
            MyCustomCounter.Add(1, new KeyValuePair<string, object>("consumidor", "ConsumidorGenerarFactura"));
        }
    }
}
