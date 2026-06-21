using System;
using System.Collections.Generic;

namespace TempestSushi.Infraestructure.Models;

public partial class RolUsuario
{
    public int IdRolUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
