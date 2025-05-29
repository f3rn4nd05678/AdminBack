using MongoDB.Driver;
using MongoDB.Bson;
using Newtonsoft.Json;
using AdminBack.Service.IService;

namespace AdminBack.Service
{
    public class ProveedorMongoService : IProveedorMongoService
    {
        private readonly IMongoCollection<BsonDocument> _catalogoCollection;
        private readonly IMongoDatabase _database;


        public ProveedorMongoService(IMongoDatabase database)
        {
            _catalogoCollection = database.GetCollection<BsonDocument>("CatalogoProveedor");
            _database = database;
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

        public async Task GuardarOrdenEnviada(string proveedor, object payload, object respuesta)
        {
            var doc = new BsonDocument
    {
        { "proveedor", proveedor },
        { "fecha", DateTime.UtcNow },
        { "payload", BsonDocument.Parse(JsonConvert.SerializeObject(payload)) },
        { "respuesta", BsonDocument.Parse(JsonConvert.SerializeObject(respuesta)) }
    };

            var collection = _database.GetCollection<BsonDocument>("OrdenProveedor");
            await collection.InsertOneAsync(doc);
        }

        public async Task GuardarTracking(string proveedor, string ordenId, object respuesta)
        {
            var doc = new BsonDocument
    {
        { "proveedor", proveedor },
        { "ordenId", ordenId },
        { "fechaConsulta", DateTime.UtcNow },
        { "respuesta", BsonDocument.Parse(JsonConvert.SerializeObject(respuesta)) }
    };

            var collection = _database.GetCollection<BsonDocument>("TrackingProveedor");
            await collection.InsertOneAsync(doc);
        }


    }
}
