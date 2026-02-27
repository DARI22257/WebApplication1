using WebApplication1.DB;

namespace WebApplication1.DTO;

public class CreateEmployeeDto
{
    public EmployeeDTO EmployeeDto { get; set; } = new();
    public ProfileDTO ProfileDto { get; set; } = new();
}