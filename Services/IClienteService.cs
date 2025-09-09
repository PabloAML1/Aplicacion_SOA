using Aplicacion_SOA.Models;

namespace Aplicacion_SOA.Services
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAllClientes();
        Task<Cliente?> GetClienteByCedula(string cedula);
        Task<Cliente> CreateCliente(Cliente cliente);
        Task<Cliente?> UpdateCliente(string cedula, Cliente cliente);
        Task<bool> DeleteCliente(string cedula);
    }
}
