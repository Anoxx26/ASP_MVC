using System;
using System.Collections.Generic;

namespace ASP_MVC.Data.Models;

public partial class Role
{
    public int Idrole { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
