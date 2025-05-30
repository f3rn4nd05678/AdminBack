namespace AdminBack.Models
{
    public class RutaEntrega
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string? Descripcion { get; set; }
        public DateTime Fecha { get; set; }

        public virtual ICollection<EntregaPedido> Entregas { get; set; } = new List<EntregaPedido>();
    }

}
