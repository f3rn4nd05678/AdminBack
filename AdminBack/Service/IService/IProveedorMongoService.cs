namespace AdminBack.Service.IService
{
    public interface IProveedorMongoService
    {
        Task GuardarCatalogo(string proveedor, object respuesta);
        Task GuardarOrdenEnviada(string nombre, object payload, object value);
        Task GuardarTracking(string nombre, string v1, object v2);
    }
}
