using System;
using System.Collections.Generic;

namespace WebApplication1.DB;

public partial class Employee
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Position { get; set; } = null!;

    public DateTime HireDate { get; set; }

    public bool IsActive { get; set; }

    public int Id { get; set; }

    public virtual ICollection<Credential> Credentials { get; set; } = new List<Credential>();
}
