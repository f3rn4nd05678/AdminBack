namespace AdminBack.Models.DTOs
{
    public class DashboardFinanzasDto
    {
        public string Mes { get; set; } = "";
        public decimal TotalFacturado { get; set; }
        public decimal TotalPagado { get; set; }
        public decimal TotalNotasCredito { get; set; }
        public decimal Pendiente { get; set; }
    }

}
