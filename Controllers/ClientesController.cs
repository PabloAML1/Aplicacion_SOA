using Aplicacion_SOA.Models;
using Aplicacion_SOA.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aplicacion_SOA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(IClienteService clienteService, ILogger<ClientesController> logger)
        {
            _clienteService = clienteService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            var clientes = await _clienteService.GetAllClientes();
            return Ok(clientes);
        }

        [HttpGet("{cedula}")]
        public async Task<ActionResult<Cliente>> GetCliente(string cedula)
        {
            var cliente = await _clienteService.GetClienteByCedula(cedula);
            if (cliente == null)
                return NotFound($"Cliente con cédula {cedula} no encontrado");

            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> CreateCliente(Cliente cliente)
        {
            var nuevoCliente = await _clienteService.CreateCliente(cliente);
            return CreatedAtAction(nameof(GetCliente), new { cedula = nuevoCliente.Cedula }, nuevoCliente);
        }

        [HttpPut("{cedula}")]
        public async Task<IActionResult> UpdateCliente(string cedula, Cliente cliente)
        {
            var clienteActualizado = await _clienteService.UpdateCliente(cedula, cliente);
            if (clienteActualizado == null)
                return NotFound($"Cliente con cédula {cedula} no encontrado");

            return Ok(clienteActualizado);
        }

        [HttpDelete("{cedula}")]
        public async Task<IActionResult> DeleteCliente(string cedula)
        {
            try
            {
                var eliminado = await _clienteService.DeleteCliente(cedula);
                if (!eliminado)
                    return NotFound($"Cliente con cédula {cedula} no encontrado");

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

    }
}

