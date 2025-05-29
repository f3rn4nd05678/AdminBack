namespace AdminBack.Models.DTOs.Cliente
{
    public class ClienteUpdateDto
    {
        public string Nombre { get; set; }
        public string? Rfc { get; set; }
        public string? Direccion { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
    }
}
