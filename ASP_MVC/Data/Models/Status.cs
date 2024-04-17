using System;
using System.Collections.Generic;

namespace ASP_MVC.Data.Models;

public partial class Status
{
    public int Idstatus { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
}
