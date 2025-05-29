using MongoDB.Driver;
using MongoDB.Bson;
using Newtonsoft.Json;
using AdminBack.Service.IService;

namespace AdminBack.Service
{
    public class ProveedorMongoService : IProveedorMongoService
    {
        private readonly IMongoCollection<BsonDocument> _catalogoCollection;

        public ProveedorMongoService(IMongoDatabase database)
        {
            _catalogoCollection = database.GetCollection<BsonDocument>("CatalogoProveedor");
        }

        public async Task GuardarCatalogo(string proveedor, object respuesta)
        {
            var doc = new BsonDocument
            {
                { "proveedor", proveedor },
                { "fecha", DateTime.UtcNow },
                { "respuesta", BsonDocument.Parse(JsonConvert.SerializeObject(respuesta)) }
            };

            await _catalogoCollection.InsertOneAsync(doc);
        }
    }
}
