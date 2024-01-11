using BANCO_AMARILLO.Conexion;
using BANCO_AMARILLO.Modelos;
using Dapper;
using Microsoft.AspNetCore.Mvc;


namespace BANCO_AMARILLO.Controllers
{
    [ApiController]
    [Route("api/informacionRed")]
    public class InformacionRedController : ControllerBase
    {
        private readonly ConsultasDapper informacionDapper;

        public InformacionRedController(ConsultasDapper informacionDapper)
        {
            this.informacionDapper = informacionDapper;
        }

        [HttpGet("BusquedaGeneral")]
        public IActionResult Busquedas()
        {
            var clientes = informacionDapper.BusquedaGeneral();
            return Ok(clientes);
        }

        [HttpGet("Excel/BusquedaGeneralExcel")]
        public IActionResult BusquedasExcel()
        {
            var clientes = informacionDapper.BusquedaGeneralExcel();
            return Ok(clientes);
        }
  

        [HttpGet("ipo/{ip}")]
        public IActionResult BusquedaPorIp(string ip)
        {

            var validacion = new Validaciones();
            if (!validacion.validacionIp(ip))
            {
                return BadRequest("La dirección IP ingresada no es válida.");
            }

            var cuenta = informacionDapper.BusquedaPorIp(ip);

            if (cuenta == null)
            {
                return NotFound();
            }

            return Ok(cuenta);
        }

        [HttpGet("idi/{id}")]
        public IActionResult BusquedaPorId(int id)
        {
            var validacion = new Validaciones();
            var validacionResult = validacion.ValidarNumeroEnteroPositivo(id);

            if (!validacionResult.Item1)
            {
                return BadRequest($"El ID ingresado no es válido. {validacionResult.Item2}");
            }

            var cuenta = informacionDapper.BusquedaPorId(id);

            if (cuenta == null)
            {
                return NotFound($"No se encontró ninguna cuenta con el ID proporcionado: {id}");
            }

            return Ok(cuenta);
        }


        [HttpGet("cb/{cb}")]
        public IActionResult BusquedaPorCB(string cb)
        {
            var validacion = new Validaciones();
            IActionResult validation = validacion.ValidateAndReturnMessage(cb, value =>
            {
                // Realiza la lógica para verificar si el registro existe.
                var cuenta = informacionDapper.BusquedaPorCB(value);
                return cuenta != null;
            });

            if (validation != null)
            {
                return validation;
            }

            // Continúa con el resto del código
            var cuenta = informacionDapper.BusquedaPorCB(cb);
            return Ok(cuenta);
        }


        [HttpGet("num_serie/{sre}")]
        public IActionResult BusquedaPorNumeroSerie(string sre)
        {
            var validacion = new Validaciones();

            var (esValido, mensaje) = validacion.ValidarNumeroSerie(sre);

            if (!esValido)
            {
                return BadRequest(mensaje);
            }

            var cuenta = informacionDapper.BusquedaPorNumeroSerie(sre);

            if (cuenta == null)
            {
                return NotFound();
            }

            return Ok(cuenta);
        }


        [HttpGet("mac/{mac}")]
        public IActionResult BusquedaPorMAC(string mac)
        {
            var validacion = new Validaciones();
            string mensaje = validacion.ValidarDireccionMAC(mac);

            if (mensaje == "La dirección MAC es válida.")
            {
                var cuenta = informacionDapper.BusquedaPorMac(mac);

                if (cuenta == null)
                {
                    return NotFound("No se encontró ningún registro con la dirección MAC proporcionada.");
                }

                return Ok(cuenta);
            }
            else
            {
                return BadRequest(mensaje);
            }
        }

        [HttpPost("Insertar")]
        public IActionResult InsertarInformacionRed([FromBody] InformacionRed informacionRed)
        {
            try
            {
                if (informacionRed == null)
                {
                    return BadRequest("Los datos de la transacción son nulos.");
                }

                // Llama a la función para insertar la información de red
                int filasAfectadas = informacionDapper.InsertarTransaccionRed(informacionRed);

                if (filasAfectadas > 0)
                {
                    return Ok("Información de red insertada correctamente");
                }
                else
                {
                    return BadRequest("Error al insertar información de red");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al insertar información de red: {ex.Message}");
            }
        }

        [HttpPut("act/{id}")]
        public IActionResult ActualizarTransaccionRed(int id, [FromBody] ActualizarRed transaccion)
        {
            var validacion = new Validaciones();

            // Validar el ID
            var resultadoValidacionId = validacion.ValidarNumeroEnteroPositivo(id);
            if (!resultadoValidacionId.Item1)
            {
                return BadRequest(resultadoValidacionId.Item2);
            }

            // Validar la transacción
            if (transaccion == null)
            {
                return BadRequest("La transacción de red no es válida.");
            }

            // Realizar la actualización
            int filasAfectadas = informacionDapper.ActualizarInformacionRed(id, transaccion);

            if (filasAfectadas > 0)
            {
                return Ok("Transacción de red actualizada correctamente.");
            }
            else
            {
                return BadRequest("Error al actualizar la transacción de red.");
            }
        }



        [HttpDelete("{id}")]
        public IActionResult EliminarInformacionRed(int id)
        {
            // Llama directamente a la función que elimina el registro por ID.
            informacionDapper.EliminarInformacionRedID(id);

            return Ok("Registro eliminado correctamente.");
        }

        [HttpDelete("Excel/{id}")]
        public IActionResult EliminarExcel(int id)
        {
            informacionDapper.EliminarInformacionExcel(id);
            return Ok("Registro eliminado correctamente. ");
        }
       

        [HttpPost("login")]
        public IActionResult Login([FromBody] Usuarios loginRequest)
        {
            if (informacionDapper.ValidarCredenciales(loginRequest.Nombre, loginRequest.Contrasena))
            {
                string token = informacionDapper.GenerarTokenJwt(loginRequest.Nombre); 
                return Ok(new { token });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("operacionCompleta")]
        public IActionResult OperacionCompleta(int id)
        {
            var result = informacionDapper.EjecutarFunctionExcel(id);

   
            return Ok(new { Resultado = result });
        }
    }
}
