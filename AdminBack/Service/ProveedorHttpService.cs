using AdminBack.Data;
using AdminBack.Models.DTOs.Proveedor;
using AdminBack.Service.IService;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AdminBack.Service
{
    public class ProveedorHttpService : IProveedorHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly AdminDbContext _context;
        private readonly IProveedorMongoService _mongoService;

        public ProveedorHttpService(HttpClient httpClient, AdminDbContext context, IProveedorMongoService mongoService)
        {
            _httpClient = httpClient;
            _context = context;
            _mongoService = mongoService;
        }

        public async Task<List<ProductoCatalogoProveedorDto>> ObtenerYGuardarCatalogo(int proveedorId)
        {
            var proveedor = await _context.Proveedores.FindAsync(proveedorId);
            if (proveedor == null || string.IsNullOrWhiteSpace(proveedor.CatalogoUrl))
                throw new Exception("Proveedor no válido o sin URL de catálogo");

            var response = await _httpClient.GetAsync(proveedor.CatalogoUrl);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error al contactar proveedor: {response.StatusCode}");

            var contenidoJson = await response.Content.ReadAsStringAsync();
            var respuestaRaw = JsonConvert.DeserializeObject<object>(contenidoJson);

            await _mongoService.GuardarCatalogo(proveedor.Nombre, respuestaRaw!);

            // Simulación de mapeo a DTO (esto depende del proveedor)
            // Puedes ajustarlo según el formato real
            var productos = JsonConvert.DeserializeObject<List<ProductoCatalogoProveedorDto>>(contenidoJson);
            return productos ?? new();
        }
        public async Task<object?> ConsultarTracking(int proveedorId, int ordenId)
        {
            var proveedor = await _context.Proveedores.FindAsync(proveedorId);
            if (proveedor == null || string.IsNullOrWhiteSpace(proveedor.TrackUrl))
                throw new Exception("Proveedor sin URL de tracking");

            var fullUrl = $"{proveedor.TrackUrl}/{ordenId}";

            var response = await _httpClient.GetAsync(fullUrl);
            var content = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<object>(content);

            await _mongoService.GuardarTracking(proveedor.Nombre, ordenId.ToString(), json!);
            return json;
        }

    }
}
