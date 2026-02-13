namespace WebApplication1.DTO;

public class CreateEmployeeDto
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Position { get; set; } = "";
    public DateTime HireDate { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;

    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = "Employee";
}