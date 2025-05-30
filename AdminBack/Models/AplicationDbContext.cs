using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AdminBack.Models;

public partial class AplicationDbContext : DbContext
{
    public AplicationDbContext()
    {
    }

    public AplicationDbContext(DbContextOptions<AplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Almacene> Almacenes { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<ConfiguracionSistema> ConfiguracionSistemas { get; set; }

    public virtual DbSet<EntradasInventario> EntradasInventarios { get; set; }

    public virtual DbSet<InventarioActual> InventarioActuals { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SalidasInventario> SalidasInventarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=databaseADMIN;Username=postgres;Password=1234");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Almacene>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("almacenes_pkey");

            entity.ToTable("almacenes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Ubicacion)
                .HasMaxLength(150)
                .HasColumnName("ubicacion");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clientes_pkey");

            entity.ToTable("clientes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Direccion).HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Rfc)
                .HasMaxLength(20)
                .HasColumnName("rfc");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<ConfiguracionSistema>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("configuracion_sistema_pkey");

            entity.ToTable("configuracion_sistema");

            entity.HasIndex(e => e.Clave, "configuracion_sistema_clave_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Clave)
                .HasMaxLength(50)
                .HasColumnName("clave");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Valor).HasColumnName("valor");
        });

        modelBuilder.Entity<EntradasInventario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("entradas_inventario_pkey");

            entity.ToTable("entradas_inventario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AlmacenId).HasColumnName("almacen_id");
            entity.Property(e => e.Cantidad)
                .HasPrecision(12, 2)
                .HasColumnName("cantidad");
            entity.Property(e => e.FechaEntrada)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_entrada");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.Referencia).HasColumnName("referencia");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Almacen).WithMany(p => p.EntradasInventarios)
                .HasForeignKey(d => d.AlmacenId)
                .HasConstraintName("entradas_inventario_almacen_id_fkey");

            entity.HasOne(d => d.Producto).WithMany(p => p.EntradasInventarios)
                .HasForeignKey(d => d.ProductoId)
                .HasConstraintName("entradas_inventario_producto_id_fkey");

            entity.HasOne(d => d.Usuario).WithMany(p => p.EntradasInventarios)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("entradas_inventario_usuario_id_fkey");
        });

        modelBuilder.Entity<InventarioActual>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("inventario_actual_pkey");

            entity.ToTable("inventario_actual");

            entity.HasIndex(e => new { e.ProductoId, e.AlmacenId }, "inventario_actual_producto_id_almacen_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AlmacenId).HasColumnName("almacen_id");
            entity.Property(e => e.Cantidad)
                .HasPrecision(12, 2)
                .HasColumnName("cantidad");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");

            entity.HasOne(d => d.Almacen).WithMany(p => p.InventarioActuals)
                .HasForeignKey(d => d.AlmacenId)
                .HasConstraintName("inventario_actual_almacen_id_fkey");

            entity.HasOne(d => d.Producto).WithMany(p => p.InventarioActuals)
                .HasForeignKey(d => d.ProductoId)
                .HasConstraintName("inventario_actual_producto_id_fkey");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("productos_pkey");

            entity.ToTable("productos");

            entity.HasIndex(e => e.Codigo, "productos_codigo_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Codigo)
                .HasMaxLength(50)
                .HasColumnName("codigo");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.PrecioUnitario)
                .HasPrecision(12, 2)
                .HasColumnName("precio_unitario");
            entity.Property(e => e.UnidadMedida)
                .HasMaxLength(10)
                .HasColumnName("unidad_medida");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("proveedores_pkey");

            entity.ToTable("proveedores");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Direccion).HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Rfc)
                .HasMaxLength(20)
                .HasColumnName("rfc");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<SalidasInventario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("salidas_inventario_pkey");

            entity.ToTable("salidas_inventario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AlmacenId).HasColumnName("almacen_id");
            entity.Property(e => e.Cantidad)
                .HasPrecision(12, 2)
                .HasColumnName("cantidad");
            entity.Property(e => e.FechaSalida)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_salida");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.Referencia).HasColumnName("referencia");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Almacen).WithMany(p => p.SalidasInventarios)
                .HasForeignKey(d => d.AlmacenId)
                .HasConstraintName("salidas_inventario_almacen_id_fkey");

            entity.HasOne(d => d.Producto).WithMany(p => p.SalidasInventarios)
                .HasForeignKey(d => d.ProductoId)
                .HasConstraintName("salidas_inventario_producto_id_fkey");

            entity.HasOne(d => d.Usuario).WithMany(p => p.SalidasInventarios)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("salidas_inventario_usuario_id_fkey");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usuarios_pkey");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Email, "usuarios_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Contrasena).HasColumnName("contrasena");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(100)
                .HasColumnName("nombre_completo");
            entity.Property(e => e.RolId).HasColumnName("rol_id");

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("usuarios_rol_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
