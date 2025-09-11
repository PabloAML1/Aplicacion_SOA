using Aplicacion_SOA.Models;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion_SOA.Services
{
    public class ProductoService : IProductoService
    {
        private readonly CatalogoContext _context;

        public ProductoService(CatalogoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Producto>> GetAllProductos()
        {
            return await _context.Productos
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<Producto?> GetProductoById(int id)
        {
            return await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Producto> CreateProducto(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task<Producto?> UpdateProducto(int id, Producto producto)
        {
            var existingProducto = await _context.Productos.FindAsync(id);
            if (existingProducto == null)
                return null;

            existingProducto.Nombre = producto.Nombre;
            existingProducto.Precio = producto.Precio;
            existingProducto.Disponible = producto.Disponible;

            await _context.SaveChangesAsync();
            return existingProducto;
        }

        public async Task<bool> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return false;

            bool tienePedidos = await _context.Pedidos
                .AnyAsync(p => p.ProductoId == id);

            if (tienePedidos)
                throw new InvalidOperationException("No se puede eliminar el producto porque está asociado a pedidos.");

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ProductoExists(int id)
        {
            return await _context.Productos.AnyAsync(p => p.Id == id);
        }
    }
}
