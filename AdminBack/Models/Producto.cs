using System;
using System.Collections.Generic;

namespace AdminBack.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? UnidadMedida { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<EntradasInventario> EntradasInventarios { get; set; } = new List<EntradasInventario>();

    public virtual ICollection<InventarioActual> InventarioActuals { get; set; } = new List<InventarioActual>();

    public virtual ICollection<SalidasInventario> SalidasInventarios { get; set; } = new List<SalidasInventario>();
}
