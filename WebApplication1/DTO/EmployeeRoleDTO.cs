namespace WebApplication1.DTO;


public class EmployeeRoleDTO
{
    public EmployeeDTO EmployeeDto { get; set; } = new();
    public RoleDto RoleDto { get; set; } = new();
}