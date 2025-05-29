using System;
using System.Collections.Generic;

namespace AdminBack.Models;

public partial class Proveedore
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Rfc { get; set; }

    public string? Direccion { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public bool? Activo { get; set; }
    public string? CatalogoUrl { get; set; }
    public string? TrackUrl { get; set; }
    public string? OrdenUrl { get; set; } 

}
