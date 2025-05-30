namespace AdminBack.Models
{
    public class Transportista
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string? Telefono { get; set; }
        public bool Activo { get; set; } = true;

        public virtual ICollection<EntregaPedido> Entregas { get; set; } = new List<EntregaPedido>();
    }

}
