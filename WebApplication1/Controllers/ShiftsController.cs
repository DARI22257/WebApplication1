using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DB;
using WebApplication1.DTO;

namespace WebApplication1.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ShiftsController : ControllerBase
{
    private readonly _1135ChubrikContext _db;

    public ShiftsController(_1135ChubrikContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<ShiftDTO>>> GetAll()
    {
        var list = await _db.Shifts
            .OrderByDescending(s => s.StartDateTime)
            .Select(s => new ShiftDTO
            {
                Id = s.Id,
                EmployeeId = s.Employeeld,
                StartDateTime = s.StartDateTime,
                EndDateTime = s.EndDateTime,
                Description = s.Description
            })
            .ToListAsync();

        return Ok(list);
    }

    [HttpGet("employee/{employeeId:int}")]
    public async Task<ActionResult<List<ShiftDTO>>> GetByEmployee(int employeeId)
    {
        var list = await _db.Shifts
            .Where(s => s.Employeeld == employeeId)
            .OrderByDescending(s => s.StartDateTime)
            .Select(s => new ShiftDTO
            {
                Id = s.Id,
                EmployeeId = s.Employeeld,
                StartDateTime = s.StartDateTime,
                EndDateTime = s.EndDateTime,
                Description = s.Description
            })
            .ToListAsync();

        return Ok(list);
    }

    [HttpPost]
    public async Task<ActionResult<ShiftDTO>> Create([FromBody] ShiftDTO dto)
    {
        var s = new Shift
        {
            Employeeld = dto.EmployeeId,
            StartDateTime = dto.StartDateTime,
            EndDateTime = dto.EndDateTime,
            Description = dto.Description
        };

        _db.Shifts.Add(s);
        await _db.SaveChangesAsync();

        dto.Id = s.Id;
        return Ok(dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ShiftDTO dto)
    {
        var s = await _db.Shifts.FirstOrDefaultAsync(x => x.Id == id);
        if (s == null) return NotFound();

        s.Employeeld = dto.EmployeeId;
        s.StartDateTime = dto.StartDateTime;
        s.EndDateTime = dto.EndDateTime;
        s.Description = dto.Description;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var s = await _db.Shifts.FirstOrDefaultAsync(x => x.Id == id);
        if (s == null) return NotFound();

        _db.Shifts.Remove(s);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}