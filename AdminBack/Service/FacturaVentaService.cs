using AdminBack.Data;
using AdminBack.Service.IService;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace AdminBack.Service
{
    public class FacturaVentaService : IFacturaVentaService
    {
        private readonly AdminDbContext _context;
        private readonly IMongoDatabase _mongo;

        public FacturaVentaService(AdminDbContext context, IMongoDatabase mongo)
        {
            _context = context;
            _mongo = mongo;
        }

        public async Task<object?> GenerarFacturaJson(int pedidoId)
        {
            var pedido = await _context.PedidosCliente
                .Include(p => p.Cliente)
                .Include(p => p.Detalles)
                .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);

            if (pedido == null) throw new Exception("Pedido no encontrado");

            var config = await _context.ConfiguracionSistemas.ToListAsync();
            string moneda = config.FirstOrDefault(c => c.Clave == "moneda")?.Valor ?? "GTQ";
            decimal ivaPorc = decimal.Parse(config.FirstOrDefault(c => c.Clave == "porcentaje_iva")?.Valor ?? "12");
            bool conIVA = config.FirstOrDefault(c => c.Clave == "mostrar_precios_con_iva")?.Valor == "true";

            var productos = pedido.Detalles.Select(d => new
            {
                nombre = d.Producto.Nombre,
                cantidad = d.Cantidad,
                precio = d.PrecioUnitario,
                total = d.Cantidad * d.PrecioUnitario
            }).ToList();

            decimal subtotal = productos.Sum(p => (decimal)p.total);
            decimal iva = conIVA ? subtotal * (ivaPorc / 100) : 0;
            decimal total = subtotal + iva;

            var factura = new
            {
                fecha = pedido.Fecha.ToString("yyyy-MM-dd"),
                pedidoId = pedido.Id,
                cliente = new
                {
                    nombre = pedido.Cliente.Nombre,
                    direccion = pedido.Cliente.Direccion,
                    email = pedido.Cliente.Email
                },
                productos,
                totales = new
                {
                    subtotal,
                    iva,
                    total,
                    moneda
                },
                observaciones = "Gracias por su compra"
            };

            // Guardar en Mongo
            var json = JsonConvert.SerializeObject(factura);
            var doc = BsonDocument.Parse(json);
            var collection = _mongo.GetCollection<BsonDocument>("FacturaVenta");
            await collection.InsertOneAsync(doc);

            // Guardar id en relacional
            pedido.FacturaIdMongo = doc["_id"].ToString();
            await _context.SaveChangesAsync();

            return factura;
        }

        public async Task<object?> ObtenerFacturaDesdeMongo(int pedidoId)
        {
            var pedido = await _context.PedidosCliente
                .FirstOrDefaultAsync(p => p.Id == pedidoId);

            if (pedido == null || string.IsNullOrWhiteSpace(pedido.FacturaIdMongo))
                throw new Exception("Factura no generada o no registrada para este pedido");

            var collection = _mongo.GetCollection<BsonDocument>("FacturaVenta");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(pedido.FacturaIdMongo));
            var doc = await collection.Find(filter).FirstOrDefaultAsync();

            if (doc == null) throw new Exception("Factura no encontrada en MongoDB");

            return BsonTypeMapper.MapToDotNetValue(doc);
        }

    }
}
