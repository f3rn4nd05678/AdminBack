namespace AdminBack.Service.IService
{
    public interface IProveedorMongoService
    {
        Task GuardarCatalogo(string proveedor, object respuesta);
    }
}
