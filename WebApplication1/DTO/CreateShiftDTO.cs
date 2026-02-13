namespace WebApplication1.DTO;

public class CreateShiftDTO
{
    public int EmployeeId { get; set; }
    public DateTime StartDateTime { get; set; } = DateTime.Now;
    public DateTime EndDateTime { get; set; } = DateTime.Now.AddHours(8);
    public string? Description { get; set; }
}