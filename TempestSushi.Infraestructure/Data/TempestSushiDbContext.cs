using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TempestSushi.Infraestructure.Models;

namespace TempestSushi.Infraestructure.Data;

public partial class TempestSushiDbContext : DbContext
{
    public TempestSushiDbContext(DbContextOptions<TempestSushiDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Carrito> Carritos { get; set; }

    public virtual DbSet<CarritoCombo> CarritoCombos { get; set; }

    public virtual DbSet<CarritoProducto> CarritoProductos { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Combo> Combos { get; set; }

    public virtual DbSet<ComboProducto> ComboProductos { get; set; }

    public virtual DbSet<EstacionCocina> EstacionCocinas { get; set; }

    public virtual DbSet<EstadoPedido> EstadoPedidos { get; set; }

    public virtual DbSet<ImagenProducto> ImagenProductos { get; set; }

    public virtual DbSet<Ingrediente> Ingredientes { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<MenuCombo> MenuCombos { get; set; }

    public virtual DbSet<MenuProducto> MenuProductos { get; set; }

    public virtual DbSet<MetodoEntrega> MetodoEntregas { get; set; }

    public virtual DbSet<MetodoPago> MetodoPagos { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<PedidoDetalleCombo> PedidoDetalleCombos { get; set; }

    public virtual DbSet<PedidoDetalleProducto> PedidoDetalleProductos { get; set; }

    public virtual DbSet<ProcesoPreparacion> ProcesoPreparacions { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<RolUsuario> RolUsuarios { get; set; }

    public virtual DbSet<SeguimientoCocina> SeguimientoCocinas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Carrito>(entity =>
        {
            entity.HasKey(e => e.IdCarrito).HasName("PK__Carrito__8B4A618C21747F3C");

            entity.ToTable("Carrito");

            entity.HasIndex(e => e.IdUsuario, "IX_Carrito_Usuario");

            entity.HasIndex(e => e.IdUsuario, "IX_Carrito_UsuarioActivo")
                .IsUnique()
                .HasFilter("([Activo]=(1))");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdUsuarioNavigation).WithOne(p => p.Carrito)
                .HasForeignKey<Carrito>(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Carrito_Usuario");
        });

        modelBuilder.Entity<CarritoCombo>(entity =>
        {
            entity.HasKey(e => e.IdCarritoCombo).HasName("PK__CarritoC__BA730A2967258FF3");

            entity.ToTable("CarritoCombo");

            entity.Property(e => e.Observaciones).HasMaxLength(300);

            entity.HasOne(d => d.IdCarritoNavigation).WithMany(p => p.CarritoCombos)
                .HasForeignKey(d => d.IdCarrito)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CarritoCombo_Carrito");

            entity.HasOne(d => d.IdComboNavigation).WithMany(p => p.CarritoCombos)
                .HasForeignKey(d => d.IdCombo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CarritoCombo_Combo");
        });

        modelBuilder.Entity<CarritoProducto>(entity =>
        {
            entity.HasKey(e => e.IdCarritoProducto).HasName("PK__CarritoP__54956F9CDC1EA27F");

            entity.ToTable("CarritoProducto");

            entity.Property(e => e.Observaciones).HasMaxLength(300);

            entity.HasOne(d => d.IdCarritoNavigation).WithMany(p => p.CarritoProductos)
                .HasForeignKey(d => d.IdCarrito)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CarritoProducto_Carrito");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.CarritoProductos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CarritoProducto_Producto");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__A3C02A10EDB170F1");

            entity.HasIndex(e => e.Nombre, "UQ__Categori__75E3EFCFF96F1CFF").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion).HasMaxLength(300);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Combo>(entity =>
        {
            entity.HasKey(e => e.IdCombo).HasName("PK__Combo__D65BF2C82C5C1ECF");

            entity.ToTable("Combo");

            entity.HasIndex(e => e.IdCategoria, "IX_Combo_Categoria");

            entity.HasIndex(e => e.Nombre, "UQ__Combo__75E3EFCFD7164005").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.Nombre).HasMaxLength(150);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Combos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Combo_Categoria");
        });

        modelBuilder.Entity<ComboProducto>(entity =>
        {
            entity.HasKey(e => new { e.IdCombo, e.IdProducto });

            entity.ToTable("ComboProducto");

            entity.Property(e => e.Cantidad).HasDefaultValue(1);

            entity.HasOne(d => d.IdComboNavigation).WithMany(p => p.ComboProductos)
                .HasForeignKey(d => d.IdCombo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComboProducto_Combo");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.ComboProductos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComboProducto_Producto");
        });

        modelBuilder.Entity<EstacionCocina>(entity =>
        {
            entity.HasKey(e => e.IdEstacionCocina).HasName("PK__Estacion__14F79E68F564E250");

            entity.ToTable("EstacionCocina");

            entity.HasIndex(e => e.Nombre, "UQ__Estacion__75E3EFCFD3922B34").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion).HasMaxLength(300);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<EstadoPedido>(entity =>
        {
            entity.HasKey(e => e.IdEstadoPedido).HasName("PK__EstadoPe__86B983711EF87072");

            entity.ToTable("EstadoPedido");

            entity.HasIndex(e => e.Nombre, "UQ__EstadoPe__75E3EFCFE0D4B4C6").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<ImagenProducto>(entity =>
        {
            entity.HasKey(e => e.IdImagenProducto).HasName("PK__ImagenPr__B5894BD9568A6D85");

            entity.ToTable("ImagenProducto");

            entity.Property(e => e.Orden).HasDefaultValue(1);
            entity.Property(e => e.UrlImagen).HasMaxLength(500);

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.ImagenProductos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImagenProducto_Producto");
        });

        modelBuilder.Entity<Ingrediente>(entity =>
        {
            entity.HasKey(e => e.IdIngrediente).HasName("PK__Ingredie__3DA4DD60B83C0911");

            entity.ToTable("Ingrediente");

            entity.HasIndex(e => e.Nombre, "UQ__Ingredie__75E3EFCF07F4C4E1").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.IdMenu).HasName("PK__Menu__4D7EA8E1207124A2");

            entity.ToTable("Menu");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.DiasDisponibles).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(150);
        });

        modelBuilder.Entity<MenuCombo>(entity =>
        {
            entity.HasKey(e => new { e.IdMenu, e.IdCombo });

            entity.ToTable("MenuCombo");

            entity.Property(e => e.Activo).HasDefaultValue(true);

            entity.HasOne(d => d.IdComboNavigation).WithMany(p => p.MenuCombos)
                .HasForeignKey(d => d.IdCombo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuCombo_Combo");

            entity.HasOne(d => d.IdMenuNavigation).WithMany(p => p.MenuCombos)
                .HasForeignKey(d => d.IdMenu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuCombo_Menu");
        });

        modelBuilder.Entity<MenuProducto>(entity =>
        {
            entity.HasKey(e => new { e.IdMenu, e.IdProducto });

            entity.ToTable("MenuProducto");

            entity.Property(e => e.Activo).HasDefaultValue(true);

            entity.HasOne(d => d.IdMenuNavigation).WithMany(p => p.MenuProductos)
                .HasForeignKey(d => d.IdMenu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuProducto_Menu");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.MenuProductos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuProducto_Producto");
        });

        modelBuilder.Entity<MetodoEntrega>(entity =>
        {
            entity.HasKey(e => e.IdMetodoEntrega).HasName("PK__MetodoEn__82DB428B5D18CA50");

            entity.ToTable("MetodoEntrega");

            entity.HasIndex(e => e.Nombre, "UQ__MetodoEn__75E3EFCFFB6E1F65").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.HasKey(e => e.IdMetodoPago).HasName("PK__MetodoPa__6F49A9BEB3EB4DF3");

            entity.ToTable("MetodoPago");

            entity.HasIndex(e => e.Nombre, "UQ__MetodoPa__75E3EFCF15C32371").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPago).HasName("PK__Pago__FC851A3A0979A2A2");

            entity.ToTable("Pago");

            entity.Property(e => e.FechaPago).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MontoPagado).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Vuelto).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdMetodoPagoNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdMetodoPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pago_MetodoPago");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pago_Pedido");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PK__Pedido__9D335DC39BAF7A31");

            entity.ToTable("Pedido");

            entity.HasIndex(e => e.IdCliente, "IX_Pedido_Cliente");

            entity.HasIndex(e => e.IdEmpleado, "IX_Pedido_Empleado");

            entity.HasIndex(e => e.IdEstadoPedido, "IX_Pedido_Estado");

            entity.HasIndex(e => e.FechaPedido, "IX_Pedido_Fecha");

            entity.Property(e => e.CostoEnvio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DireccionEntrega).HasMaxLength(500);
            entity.Property(e => e.FechaPedido).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Impuesto).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.PedidoIdClienteNavigations)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedido_Cliente");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.PedidoIdEmpleadoNavigations)
                .HasForeignKey(d => d.IdEmpleado)
                .HasConstraintName("FK_Pedido_Empleado");

            entity.HasOne(d => d.IdEstadoPedidoNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdEstadoPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedido_Estado");

            entity.HasOne(d => d.IdMetodoEntregaNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdMetodoEntrega)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedido_MetodoEntrega");
        });

        modelBuilder.Entity<PedidoDetalleCombo>(entity =>
        {
            entity.HasKey(e => e.IdPedidoDetalleCombo).HasName("PK__PedidoDe__3C463AFA9D2D0608");

            entity.ToTable("PedidoDetalleCombo");

            entity.HasIndex(e => e.IdPedido, "IX_PedidoDetalleCombo_Pedido");

            entity.Property(e => e.Observaciones).HasMaxLength(300);
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdComboNavigation).WithMany(p => p.PedidoDetalleCombos)
                .HasForeignKey(d => d.IdCombo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PedidoDetalleCombo_Combo");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.PedidoDetalleCombos)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PedidoDetalleCombo_Pedido");
        });

        modelBuilder.Entity<PedidoDetalleProducto>(entity =>
        {
            entity.HasKey(e => e.IdPedidoDetalleProducto).HasName("PK__PedidoDe__DA48909293513B1C");

            entity.ToTable("PedidoDetalleProducto");

            entity.HasIndex(e => e.IdPedido, "IX_PedidoDetalleProducto_Pedido");

            entity.Property(e => e.Observaciones).HasMaxLength(300);
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.PedidoDetalleProductos)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PedidoDetalleProducto_Pedido");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.PedidoDetalleProductos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PedidoDetalleProducto_Producto");
        });

        modelBuilder.Entity<ProcesoPreparacion>(entity =>
        {
            entity.HasKey(e => e.IdProcesoPreparacion).HasName("PK__ProcesoP__3C7D27C76362381C");

            entity.ToTable("ProcesoPreparacion");

            entity.HasIndex(e => new { e.IdProducto, e.NumeroPaso }, "UQ_ProcesoPreparacion_ProductoPaso").IsUnique();

            entity.Property(e => e.DescripcionPaso).HasMaxLength(300);

            entity.HasOne(d => d.IdEstacionCocinaNavigation).WithMany(p => p.ProcesoPreparacions)
                .HasForeignKey(d => d.IdEstacionCocina)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcesoPreparacion_Estacion");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.ProcesoPreparacions)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcesoPreparacion_Producto");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__09889210DA73BD2C");

            entity.ToTable("Producto");

            entity.HasIndex(e => e.IdCategoria, "IX_Producto_Categoria");

            entity.HasIndex(e => e.Nombre, "UQ__Producto__75E3EFCFB2B6C228").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.Nombre).HasMaxLength(150);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_Categoria");

            entity.HasMany(d => d.IdIngredientes).WithMany(p => p.IdProductos)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductoIngrediente",
                    r => r.HasOne<Ingrediente>().WithMany()
                        .HasForeignKey("IdIngrediente")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProductoIngrediente_Ingrediente"),
                    l => l.HasOne<Producto>().WithMany()
                        .HasForeignKey("IdProducto")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProductoIngrediente_Producto"),
                    j =>
                    {
                        j.HasKey("IdProducto", "IdIngrediente");
                        j.ToTable("ProductoIngrediente");
                    });
        });

        modelBuilder.Entity<RolUsuario>(entity =>
        {
            entity.HasKey(e => e.IdRolUsuario).HasName("PK__RolUsuar__3FC7F91F1F92DB73");

            entity.ToTable("RolUsuario");

            entity.HasIndex(e => e.Nombre, "UQ__RolUsuar__75E3EFCF19574281").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<SeguimientoCocina>(entity =>
        {
            entity.HasKey(e => e.IdSeguimientoCocina).HasName("PK__Seguimie__9B87579A917AA226");

            entity.ToTable("SeguimientoCocina");

            entity.HasIndex(e => e.IdPedidoDetalleProducto, "IX_SeguimientoCocina_DetalleProducto");

            entity.HasOne(d => d.IdEstacionCocinaNavigation).WithMany(p => p.SeguimientoCocinas)
                .HasForeignKey(d => d.IdEstacionCocina)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SeguimientoCocina_Estacion");

            entity.HasOne(d => d.IdPedidoDetalleProductoNavigation).WithMany(p => p.SeguimientoCocinas)
                .HasForeignKey(d => d.IdPedidoDetalleProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SeguimientoCocina_PedidoDetalleProducto");

            entity.HasOne(d => d.IdUsuarioCocinaNavigation).WithMany(p => p.SeguimientoCocinas)
                .HasForeignKey(d => d.IdUsuarioCocina)
                .HasConstraintName("FK_SeguimientoCocina_Usuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF97D08B89F4");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.IdRolUsuario, "IX_Usuario_Rol");

            entity.HasIndex(e => e.Correo, "UQ__Usuario__60695A199E537FE1").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Apellidos).HasMaxLength(150);
            entity.Property(e => e.Correo).HasMaxLength(150);
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(500);
            entity.Property(e => e.Telefono).HasMaxLength(30);

            entity.HasOne(d => d.IdRolUsuarioNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRolUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_RolUsuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
