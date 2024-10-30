using MassTransit;
using Mensajes;
using Microsoft.AspNetCore.Mvc;

namespace Ordenes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenesController : ControllerBase
    {
        public readonly IPublishEndpoint _publishEndpoint;

        public OrdenesController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public string Post()
        {
            _publishEndpoint.Publish(new IniciarProcesoOrden
            {
                IdProcesoOrden = Guid.NewGuid()
            });
            return "proceso  iniciado";
        }
    }
}