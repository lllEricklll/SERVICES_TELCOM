using BANCO_AMARILLO.Conexion;
using BANCO_AMARILLO.Modelos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BANCO_AMARILLO.Controllers
{
    [ApiController]
    [Route("api/ClientesAma")]
    public class ClientesAmaController : ControllerBase
    {
        private readonly ConsultasDapper personaRepository;

        public ClientesAmaController(ConsultasDapper personaRepository)
        {
            this.personaRepository = personaRepository;
        }

       /* [HttpGet("ClientesAma")]
        [EnableCors("AllowAnyOrigin")]
        public IActionResult GetAllBancoAmarillo()
        {
            var clientes = personaRepository.GetAllBancoAmarillo();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public IActionResult GetByIdAmarillo(int id)
        {
            var cliente = personaRepository.GetByIdAmarillo(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return Ok(cliente);
        }*/

        [HttpPost]
        public IActionResult InsertarCliente([FromBody] ClientesAma cliente)
        {
            if (cliente == null)
            {
                return BadRequest();
            }

            personaRepository.InsertarCliente(cliente);

            return Ok("Cliente creado correctamente.");
        }

        [HttpPut("{id}")]
        public IActionResult ActualizarCliente(int id, [FromBody] ClientesAma cliente)
        {
            if (cliente == null)
            {
                return BadRequest();
            }

            if (personaRepository.GetByIdAmarillo(id) == null)
            {
                return NotFound();
            }

            cliente.id_cliente = id;
            personaRepository.ActualizarCliente(cliente);

            return Ok("Cliente actualizado correctamente.");
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarCliente(int id)
        {
            var cliente = personaRepository.GetByIdAmarillo(id);
                
            if (cliente == null)
            {
                return NotFound();
            }

            personaRepository.EliminarCliente(id);

            return Ok("Cliente eliminado correctamente.");
        }

    }
}
