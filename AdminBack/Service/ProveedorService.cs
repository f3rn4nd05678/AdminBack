using AdminBack.Data;
using AdminBack.Models;
using AdminBack.Models.DTOs.Proveedor;
using AdminBack.Service.IService;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AdminBack.Service
{
    public class ProveedorService : IProveedorService
    {
        private readonly AdminDbContext _context;
        private readonly IMongoDatabase _database;

        public ProveedorService(AdminDbContext context, IMongoDatabase database)
        {
            _context = context;
            _database = database;
        }

        public async Task<List<ProveedorDto>> ObtenerTodos()
        {
            return await _context.Proveedores
                .Where(p => p.Activo ?? true)
                .Select(p => new ProveedorDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Email = p.Email,
                    Telefono = p.Telefono,
                    CatalogoUrl = p.CatalogoUrl,
                    TrackUrl = p.TrackUrl,
                    Activo = p.Activo ?? true
                })
                .ToListAsync();
        }

        public async Task<bool> Crear(ProveedorCreateDto dto)
        {
            _context.Proveedores.Add(new Proveedore
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Telefono = dto.Telefono,
                CatalogoUrl = dto.CatalogoUrl,
                TrackUrl = dto.TrackUrl,
                Activo = true
            });

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Actualizar(int id, ProveedorUpdateDto dto)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null) return false;

            proveedor.Nombre = dto.Nombre;
            proveedor.Email = dto.Email;
            proveedor.Telefono = dto.Telefono;
            proveedor.CatalogoUrl = dto.CatalogoUrl;
            proveedor.TrackUrl = dto.TrackUrl;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Desactivar(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null) return false;

            proveedor.Activo = false;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task GuardarTracking(string proveedor, string ordenId, object respuesta)
        {
            var doc = new BsonDocument
    {
        { "proveedor", proveedor },
        { "ordenId", ordenId },
        { "fechaConsulta", DateTime.UtcNow },
        { "respuesta", BsonDocument.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(respuesta)) }
    };

            var collection = _database.GetCollection<BsonDocument>("TrackingProveedor");
            await collection.InsertOneAsync(doc);
        }

    }
}
