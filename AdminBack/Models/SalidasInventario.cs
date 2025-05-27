using System;
using System.Collections.Generic;

namespace AdminBack.Models;

public partial class SalidasInventario
{
    public int Id { get; set; }

    public int ProductoId { get; set; }

    public int AlmacenId { get; set; }

    public decimal Cantidad { get; set; }

    public DateTime? FechaSalida { get; set; }

    public string? Referencia { get; set; }

    public int? UsuarioId { get; set; }

    public virtual Almacene Almacen { get; set; } = null!;

    public virtual Producto Producto { get; set; } = null!;

    public virtual Usuario? Usuario { get; set; }
}
