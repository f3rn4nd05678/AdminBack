namespace AdminBack.Models
{
    public class EntregaPedido
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int TransportistaId { get; set; }
        public int RutaId { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public DateTime FechaAsignacion { get; set; }

        public virtual PedidoCliente Pedido { get; set; } = null!;
        public virtual Transportista Transportista { get; set; } = null!;
        public virtual RutaEntrega Ruta { get; set; } = null!;
    }

}
