using System;

namespace AvaloniaApplication2.ViewModels;

public class EmployeesViewModel
{
    public object FirstName { get; set; } = null!;

    public object LastName { get; set; } = null!;

    public object Position { get; set; } = null!;

    public DateTime HireDate { get; set; }

    public bool IsActive { get; set; }

    public int Id { get; set; }
    
}