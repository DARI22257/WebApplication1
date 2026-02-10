using System;
using System.Collections.Generic;

namespace WebApplication1.DB;

public partial class Shift
{
    public int Employeeld { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public string? Description { get; set; }

    public int Id { get; set; }

    public virtual ICollection<Credential> Credentials { get; set; } = new List<Credential>();
}
