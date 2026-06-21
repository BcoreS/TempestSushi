using System;
using System.Collections.Generic;

namespace TempestSushi.Infraestructure.Models;

public partial class MenuCombo
{
    public int IdMenu { get; set; }

    public int IdCombo { get; set; }

    public bool Activo { get; set; }

    public virtual Combo IdComboNavigation { get; set; } = null!;

    public virtual Menu IdMenuNavigation { get; set; } = null!;
}
