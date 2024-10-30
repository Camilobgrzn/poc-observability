using MassTransit;

namespace Procesos
{
    public class OrdenState : SagaStateMachineInstance, ISagaVersion
    {
        public Guid CorrelationId { get; set; }
        public int Version { get; set; }

        public string CurrentState { get; set; }
    }
}