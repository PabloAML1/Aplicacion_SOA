using Aplicacion_SOA.Models;
using Aplicacion_SOA.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoProductos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;
        private readonly ILogger<ProductosController> _logger;

        public ProductosController(IProductoService productoService, ILogger<ProductosController> logger)
        {
            _productoService = productoService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            try
            {
                var productos = await _productoService.GetAllProductos();
                return Ok(productos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            try
            {
                var producto = await _productoService.GetProductoById(id);
                if (producto == null)
                {
                    return NotFound($"Producto con ID {id} no encontrado");
                }
                return Ok(producto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener producto {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> CreateProducto(Producto producto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var nuevoProducto = await _productoService.CreateProducto(producto);
                return CreatedAtAction(nameof(GetProducto), new { id = nuevoProducto.Id }, nuevoProducto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear producto");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducto(int id, Producto producto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var productoActualizado = await _productoService.UpdateProducto(id, producto);
                if (productoActualizado == null)
                {
                    return NotFound($"Producto con ID {id} no encontrado");
                }

                return Ok(productoActualizado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar producto {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            try
            {
                var eliminado = await _productoService.DeleteProducto(id);
                if (!eliminado)
                {
                    return NotFound($"Producto con ID {id} no encontrado");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar producto {Id}", id);
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
