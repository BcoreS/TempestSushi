using System;
using System.Collections.Generic;

namespace TempestSushi.Infraestructure.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public int IdRolUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Apellidos { get; set; }

    public string Correo { get; set; } = null!;

    public string? Telefono { get; set; }

    public string PasswordHash { get; set; } = null!;

    public bool DebeCambiarPassword { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual Carrito? Carrito { get; set; }

    public virtual RolUsuario IdRolUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Pedido> PedidoIdClienteNavigations { get; set; } = new List<Pedido>();

    public virtual ICollection<Pedido> PedidoIdEmpleadoNavigations { get; set; } = new List<Pedido>();

    public virtual ICollection<SeguimientoCocina> SeguimientoCocinas { get; set; } = new List<SeguimientoCocina>();
}
