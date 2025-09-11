using Aplicacion_SOA.Models;
using Aplicacion_SOA.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace Aplicacion_SOA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidosService _pedidosService;
        private readonly ILogger<PedidosController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public PedidosController(
            IPedidosService pedidosService,
            ILogger<PedidosController> logger,
            IHttpClientFactory httpClientFactory)
        {
            _pedidosService = pedidosService;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
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
            try
            {
                var productosClient = _httpClientFactory.CreateClient("ProductosService");
                var clientesClient = _httpClientFactory.CreateClient("ClientesService");

                // Validar cliente
                var clienteResp = await clientesClient.GetAsync($"clientes/{pedido.ClienteId}");
                if (!clienteResp.IsSuccessStatusCode)
                    return BadRequest("El cliente no existe");

                // Validar producto
                var productoResp = await productosClient.GetAsync($"productos/{pedido.ProductoId}");
                if (!productoResp.IsSuccessStatusCode)
                    return BadRequest("El producto no existe");

                var producto = await productoResp.Content.ReadFromJsonAsync<Producto>();
                if (producto == null)
                    return BadRequest("Error al obtener los datos del producto");

                if (!producto.Disponible)
                    return BadRequest("El producto no está disponible");

                // Guardar pedido en BD
                var nuevoPedido = await _pedidosService.CreatePedido(pedido);
                return CreatedAtAction(nameof(GetPedido), new { id = nuevoPedido.Id }, nuevoPedido);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear pedido");
                return StatusCode(500, "Error interno del servidor");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePedido(int id, Pedido pedido)
        {
            try
            {
                var productosClient = _httpClientFactory.CreateClient("ProductosService");
                var clientesClient = _httpClientFactory.CreateClient("ClientesService");

                // Validar cliente
                var clienteResp = await clientesClient.GetAsync($"clientes/{pedido.ClienteId}");
                if (!clienteResp.IsSuccessStatusCode)
                    return BadRequest("El cliente no existe");

                // Validar producto
                var productoResp = await productosClient.GetAsync($"productos/{pedido.ProductoId}");
                if (!productoResp.IsSuccessStatusCode)
                    return BadRequest("El producto no existe");

                var producto = await productoResp.Content.ReadFromJsonAsync<Producto>();
                if (producto == null)
                    return BadRequest("Error al obtener los datos del producto");

                if (!producto.Disponible)
                    return BadRequest("El producto no está disponible");

                // Actualizar pedido en BD
                var pedidoActualizado = await _pedidosService.UpdatePedido(id, pedido);
                if (pedidoActualizado == null)
                    return NotFound($"Pedido con ID {id} no encontrado");

                return Ok(pedidoActualizado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar pedido {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
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
