using Aplicacion_SOA.Models;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion_SOA.Services
{
    public class ClienteService : IClienteService
    {
        private readonly CatalogoContext _context;

        public ClienteService(CatalogoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetAllClientes()
        {
            return await _context.Clientes
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        public async Task<Cliente?> GetClienteByCedula(string cedula)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.Cedula == cedula);
        }

        public async Task<Cliente> CreateCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<Cliente?> UpdateCliente(string cedula, Cliente cliente)
        {
            var existingCliente = await _context.Clientes.FindAsync(cedula);
            if (existingCliente == null)
                return null;

            existingCliente.Nombre = cliente.Nombre;
            existingCliente.Apellido = cliente.Apellido;
            existingCliente.Correo = cliente.Correo;

            await _context.SaveChangesAsync();
            return existingCliente;
        }

        public async Task<bool> DeleteCliente(string cedula)
        {
            var cliente = await _context.Clientes.FindAsync(cedula);
            if (cliente == null)
                return false;

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
