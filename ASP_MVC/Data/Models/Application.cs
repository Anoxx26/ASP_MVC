using System;
using System.Collections.Generic;

namespace ASP_MVC.Data.Models;

public partial class Application
{
    public int IdApplication { get; set; }

    public int? Status { get; set; }

    public DateOnly? DateAddition { get; set; }

    public string? FullnameClient { get; set; }

    public string? Comment { get; set; }

    public string? WorkStatus { get; set; }

    public int? IdStaff { get; set; }

    public int? IdProblem { get; set; }

    public string? NameEquipment { get; set; }

    public decimal? Cost { get; set; }

    public DateOnly? DateEnd { get; set; }

    public int? TimeWork { get; set; }

    public virtual Problem? IdProblemNavigation { get; set; }

    public virtual Staff? IdStaffNavigation { get; set; }

    public virtual Status? StatusNavigation { get; set; }
}
