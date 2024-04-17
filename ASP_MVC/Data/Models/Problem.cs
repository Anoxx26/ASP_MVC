using System;
using System.Collections.Generic;

namespace ASP_MVC.Data.Models;

public partial class Problem
{
    public int IdProblem { get; set; }

    public string? NameProblem { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
}
