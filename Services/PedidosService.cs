using Aplicacion_SOA.Models;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion_SOA.Services
{
    public class PedidosService : IPedidosService
    {
        private readonly CatalogoContext _context;

        public PedidosService(CatalogoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pedido>> GetAllPedidos()
        {
            return await _context.Pedidos
                .OrderBy(p => p.Id)
                .ToListAsync();
        }

        public async Task<Pedido?> GetPedidoById(int id)
        {
            return await _context.Pedidos
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Pedido> CreatePedido(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
            return pedido;
        }

        public async Task<Pedido?> UpdatePedido(int id, Pedido pedido)
        {
            var existingPedido = await _context.Pedidos.FindAsync(id);
            if (existingPedido == null)
                return null;

            existingPedido.ProductoId = pedido.ProductoId;
            existingPedido.ClienteId = pedido.ClienteId;
            existingPedido.Cantidad = pedido.Cantidad;

            await _context.SaveChangesAsync();
            return existingPedido;
        }

        public async Task<bool> DeletePedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
                return false;

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
