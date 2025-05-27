using System;
using System.Collections.Generic;

namespace AdminBack.Models;

public partial class Almacene
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Ubicacion { get; set; }

    public string? Descripcion { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<EntradasInventario> EntradasInventarios { get; set; } = new List<EntradasInventario>();

    public virtual ICollection<InventarioActual> InventarioActuals { get; set; } = new List<InventarioActual>();

    public virtual ICollection<SalidasInventario> SalidasInventarios { get; set; } = new List<SalidasInventario>();
}
