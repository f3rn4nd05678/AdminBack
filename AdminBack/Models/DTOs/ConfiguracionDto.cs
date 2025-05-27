namespace AdminBack.Models.DTOs.Config
{
    public class ConfiguracionDto
    {
        public int Id { get; set; }
        public string Clave { get; set; }
        public string Valor { get; set; }
        public string? Descripcion { get; set; }
    }
}
