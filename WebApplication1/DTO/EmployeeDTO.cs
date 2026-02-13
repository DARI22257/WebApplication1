namespace WebApplication1.DTO;

public class EmployeeDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Position { get; set; } = "";
    public DateTime HireDate { get; set; }
    public bool IsActive { get; set; }

    public string Fio => $"{LastName} {FirstName}";
}