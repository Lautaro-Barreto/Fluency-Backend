using System;
using System.Collections.Generic;

namespace Persistence.Models;

public partial class NivelIngle
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
