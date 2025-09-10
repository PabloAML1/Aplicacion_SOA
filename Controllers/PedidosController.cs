using Aplicacion_SOA.Models;
using Aplicacion_SOA.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aplicacion_SOA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidosService _pedidosService;
        private readonly ILogger<PedidosController> _logger;

        public PedidosController(IPedidosService pedidosService, ILogger<PedidosController> logger)
        {
            _pedidosService = pedidosService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
        {
            var pedidos = await _pedidosService.GetAllPedidos();
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(int id)
        {
            var pedido = await _pedidosService.GetPedidoById(id);
            if (pedido == null)
                return NotFound($"Pedido con ID {id} no encontrado");

            return Ok(pedido);
        }

        [HttpPost]
        public async Task<ActionResult<Pedido>> CreatePedido(Pedido pedido)
        {
            var nuevoPedido = await _pedidosService.CreatePedido(pedido);
            return CreatedAtAction(nameof(GetPedido), new { id = nuevoPedido.Id }, nuevoPedido);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePedido(int id, Pedido pedido)
        {
            var pedidoActualizado = await _pedidosService.UpdatePedido(id, pedido);
            if (pedidoActualizado == null)
                return NotFound($"Pedido con ID {id} no encontrado");

            return Ok(pedidoActualizado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(int id)
        {
            var eliminado = await _pedidosService.DeletePedido(id);
            if (!eliminado)
                return NotFound($"Pedido con ID {id} no encontrado");

            return NoContent();
        }
    }
}
