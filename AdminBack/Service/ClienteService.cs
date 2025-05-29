using AdminBack.Data;
using AdminBack.Models;
using AdminBack.Models.DTOs.Cliente;
using AdminBack.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace AdminBack.Service
{
    public class ClienteService : IClienteService
    {
        private readonly AdminDbContext _context;

        public ClienteService(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClienteDto>> ObtenerTodos()
        {
            return await _context.Clientes
                .Where(c => c.Activo ?? true)
                .Select(c => new ClienteDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Rfc = c.Rfc,
                    Direccion = c.Direccion,
                    Email = c.Email,
                    Telefono = c.Telefono,
                    Activo = c.Activo ?? true
                })
                .ToListAsync();
        }

        public async Task<bool> Crear(ClienteCreateDto dto)
        {
            _context.Clientes.Add(new Cliente
            {
                Nombre = dto.Nombre,
                Rfc = dto.Rfc,
                Direccion = dto.Direccion,
                Email = dto.Email,
                Telefono = dto.Telefono,
                Activo = true
            });

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Actualizar(int id, ClienteUpdateDto dto)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;

            cliente.Nombre = dto.Nombre;
            cliente.Rfc = dto.Rfc;
            cliente.Direccion = dto.Direccion;
            cliente.Email = dto.Email;
            cliente.Telefono = dto.Telefono;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Desactivar(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;

            cliente.Activo = false;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
