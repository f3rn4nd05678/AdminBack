using System;
using System.Collections.Generic;

namespace AdminBack.Models;

public partial class ConfiguracionSistema
{
    public int Id { get; set; }

    public string Clave { get; set; } = null!;

    public string Valor { get; set; } = null!;

    public string? Descripcion { get; set; }
}
