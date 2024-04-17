using System;
using System.Collections.Generic;

namespace ASP_MVC.Data.Models;

public partial class Staff
{
    public int IdStaff { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public int? Role { get; set; }

    public string? Fullname { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual Role? RoleNavigation { get; set; }
}
