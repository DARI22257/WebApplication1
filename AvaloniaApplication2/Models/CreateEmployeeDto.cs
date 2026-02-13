namespace AvaloniaApplication2.Models;

public class CreateEmployeeDto
{
    public EmployeeDto Employee { get; set; }
    public CredentialDto? Credential { get; set; }
}