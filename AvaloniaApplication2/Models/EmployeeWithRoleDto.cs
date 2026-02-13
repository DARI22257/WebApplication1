using System.Text.Json.Serialization;

namespace AvaloniaApplication2.Models;

public class EmployeeWithRoleDto
{
    [JsonPropertyName("employeeDto")]
    public EmployeeDto Employee { get; set; }
    
    [JsonPropertyName("roleDto")]
    public RoleDto Role { get; set; }
}