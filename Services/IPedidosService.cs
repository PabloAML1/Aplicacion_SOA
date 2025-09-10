using Aplicacion_SOA.Models;

namespace Aplicacion_SOA.Services
{
    public interface IPedidosService
    {
        Task<IEnumerable<Pedido>> GetAllPedidos();
        Task<Pedido?> GetPedidoById(int id);
        Task<Pedido> CreatePedido(Pedido pedido);
        Task<Pedido?> UpdatePedido(int id, Pedido pedido);
        Task<bool> DeletePedido(int id);
    }
}
