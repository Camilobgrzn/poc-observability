#nullable disable
using MassTransit;
using Mensajes;
using Mensajes.Facturacion;
using Mensajes.Inventario;
using Mensajes.Ordenes;

namespace Procesos
{
    public class OrdenMachineState : MassTransitStateMachine<OrdenState>
    {
        public State CreandoOrden { get; private set; }
        public State ValidandoInventario { get; private set; }
        public State GenerandoFactura { get; private set; }
        public Event<OrdenCreada> OrdenCreadaEvent { get; private set; }
        public Event<IniciarProcesoOrden> IniciarProcesoOrdenEvent { get; private set; }
        public Event<InventarioValidado> InventarioValidadoEvent { get; private set; }
        public Event<FacturaGenerada> FacturaGeneradaEvent { get; private set; }


        public OrdenMachineState()
        {
            InstanceState(x => x.CurrentState);

            Event(() => IniciarProcesoOrdenEvent, x => x.CorrelateById(m => m.Message.IdProcesoOrden).SelectId(m => m.Message.IdProcesoOrden));
            Event(() => OrdenCreadaEvent, x => x.CorrelateById(m => m.Message.IdOrden).SelectId(m => m.Message.IdOrden));
            Event(() => InventarioValidadoEvent, x => x.CorrelateById(m => m.Message.IdOrden).SelectId(m => m.Message.IdOrden));
            Event(() => FacturaGeneradaEvent, x => x.CorrelateById(m => m.Message.IdOrden).SelectId(m => m.Message.IdOrden));

            Initially(
                When(IniciarProcesoOrdenEvent)
                    .Then(context =>
                    {
                        context.Publish(new CrearOrden() { Id = context.Message.IdProcesoOrden });
                    })
                    .TransitionTo(CreandoOrden)
            );

            During(CreandoOrden,
                When(OrdenCreadaEvent)
                    .Then(context =>
                    {
                        context.Publish(new ValidarInventario() { IdOrden = context.Message.IdOrden });
                    })
                    .TransitionTo(ValidandoInventario)
            );

            During(ValidandoInventario,
                When(InventarioValidadoEvent)
                    .Then(context =>
                    {
                        context.Publish(new GenerarFactura() { IdOrden = context.Message.IdOrden });
                    })
                    .TransitionTo(GenerandoFactura)
            );

            During(GenerandoFactura,
                When(FacturaGeneradaEvent)
                    .Finalize()
                );


            SetCompletedWhenFinalized();
        }
    }
}
