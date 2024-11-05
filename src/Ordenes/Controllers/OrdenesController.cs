using MassTransit;
using Mensajes;
using Microsoft.AspNetCore.Mvc;

namespace Ordenes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenesController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public OrdenesController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public string Post()
        {
            Guid idProcesoOrden = Guid.NewGuid();
            _publishEndpoint.Publish(new IniciarProcesoOrden
            {
                IdProcesoOrden = idProcesoOrden
            });
            return $"proceso  iniciado: {idProcesoOrden}";
        }
    }
}