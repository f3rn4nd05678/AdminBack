using AdminBack.Data;
using AdminBack.Models;
using AdminBack.Models.DTOs.OrdenCompra;
using AdminBack.Service.IService;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using System.Text;
using Newtonsoft.Json;

namespace AdminBack.Service
{
    public class OrdenCompraService : IOrdenCompraService
    {
        private readonly AdminDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IProveedorMongoService _mongoService;

        public OrdenCompraService(
            AdminDbContext context,
            HttpClient httpClient,
            IProveedorMongoService mongoService)
        {
            _context = context;
            _httpClient = httpClient;
            _mongoService = mongoService;
        }


        public async Task<List<OrdenCompraDto>> ObtenerTodas()
        {
            return await _context.OrdenesCompra
                .Include(o => o.Proveedor)
                .Include(o => o.Detalles)
                .ThenInclude(d => d.Producto)
                .OrderByDescending(o => o.FechaCreacion)
                .Select(o => new OrdenCompraDto
                {
                    Id = o.Id,
                    Proveedor = o.Proveedor.Nombre,
                    Estado = o.Estado,
                    FechaCreacion = o.FechaCreacion,
                    TotalEstimado = o.TotalEstimado,
                    Enviada = o.Enviada,
                    Detalles = o.Detalles.Select(d => new DetalleOrdenCompraDto
                    {
                        ProductoId = d.ProductoId,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<bool> Crear(OrdenCompraCreateDto dto)
        {
            var orden = new OrdenCompra
            {
                ProveedorId = dto.ProveedorId,
                FechaCreacion = DateTime.UtcNow,
                Estado = "Pendiente",
                TotalEstimado = dto.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario),
                Enviada = false
            };

            _context.OrdenesCompra.Add(orden);
            await _context.SaveChangesAsync();

            var detalles = dto.Detalles.Select(d => new DetalleOrdenCompra
            {
                OrdenId = orden.Id,
                ProductoId = d.ProductoId,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario
            }).ToList();

            _context.DetallesOrdenCompra.AddRange(detalles);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Aprobar(int id, int almacenId)
        {
            var orden = await _context.OrdenesCompra
                .Include(o => o.Detalles)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orden == null || orden.Estado != "Pendiente")
                return false;

            orden.Estado = "Aprobada";
            var proveedor = await _context.Proveedores.FindAsync(orden.ProveedorId);

            if (!string.IsNullOrWhiteSpace(proveedor?.OrdenUrl))
            {
                var payload = new
                {
                    ordenId = orden.Id,
                    productos = orden.Detalles.Select(d => new
                    {
                        productoId = d.ProductoId,
                        cantidad = d.Cantidad,
                        precio = d.PrecioUnitario
                    })
                };

                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                var jsonPayload = new StringContent(jsonString, Encoding.UTF8, "application/json");



                var response = await _httpClient.PostAsync(proveedor.OrdenUrl, jsonPayload);
                var content = await response.Content.ReadAsStringAsync();
                var respuestaRaw = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(content);

                await _mongoService.GuardarOrdenEnviada(proveedor.Nombre, payload, respuestaRaw!);

                orden.Enviada = true;
            }

            foreach (var d in orden.Detalles)
            {
                var inventario = await _context.InventarioActuals.FirstOrDefaultAsync(i =>
                    i.ProductoId == d.ProductoId && i.AlmacenId == almacenId); 

                if (inventario != null)
                    inventario.Cantidad += d.Cantidad;
                else
                    _context.InventarioActuals.Add(new InventarioActual
                    {
                        ProductoId = d.ProductoId,
                        AlmacenId = 1,
                        Cantidad = d.Cantidad
                    });
            }

            return await _context.SaveChangesAsync() > 0;
        }

    }
}
