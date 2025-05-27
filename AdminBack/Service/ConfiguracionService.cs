using AdminBack.Data;
using AdminBack.Models.DTOs.Config;
using AdminBack.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace AdminBack.Service
{
    public class ConfiguracionService : IConfiguracionService
    {
        private readonly AdminDbContext _context;

        public ConfiguracionService(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<List<ConfiguracionDto>> ObtenerTodo()
        {
            return await _context.ConfiguracionSistemas
                .Select(c => new ConfiguracionDto
                {
                    Id = c.Id,
                    Clave = c.Clave,
                    Valor = c.Valor,
                    Descripcion = c.Descripcion
                }).ToListAsync();
        }

        public async Task<ConfiguracionDto?> ObtenerPorClave(string clave)
        {
            var c = await _context.ConfiguracionSistemas.FirstOrDefaultAsync(x => x.Clave == clave);
            return c == null ? null : new ConfiguracionDto
            {
                Id = c.Id,
                Clave = c.Clave,
                Valor = c.Valor,
                Descripcion = c.Descripcion
            };
        }

        public async Task<bool> Crear(ConfiguracionCreateDto dto)
        {
            var existe = await _context.ConfiguracionSistemas.AnyAsync(c => c.Clave == dto.Clave);
            if (existe) return false;

            _context.ConfiguracionSistemas.Add(new Models.ConfiguracionSistema
            {
                Clave = dto.Clave,
                Valor = dto.Valor,
                Descripcion = dto.Descripcion
            });

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Actualizar(string clave, string nuevoValor)
        {
            var config = await _context.ConfiguracionSistemas.FirstOrDefaultAsync(c => c.Clave == clave);
            if (config == null) return false;

            config.Valor = nuevoValor;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
