using System;
using System.Collections.Generic;

namespace AdminBack.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public bool? Activo { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public int? RolId { get; set; }

    public virtual ICollection<EntradasInventario> EntradasInventarios { get; set; } = new List<EntradasInventario>();

    public virtual Role? Rol { get; set; }

    public virtual ICollection<SalidasInventario> SalidasInventarios { get; set; } = new List<SalidasInventario>();
}
