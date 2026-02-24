using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DB;
using WebApplication1.DTO;

namespace WebApplication1.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly _1135ChubrikContext _db;

    public EmployeesController(_1135ChubrikContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<EmployeeDTO>>> GetAll()
    {
        var list = await _db.Employees
            .OrderBy(e => e.Id)
            .Select(e => new EmployeeDTO
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Position = e.Position,
                HireDate = e.HireDate,
                IsActive = e.IsActive
            })
            .ToListAsync();

        return Ok(list);
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDTO>> Create([FromBody] EmployeeDTO dto)
    {
        var e = new Employee
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Position = dto.Position,
            HireDate = dto.HireDate,
            IsActive = dto.IsActive
        };

        _db.Employees.Add(e);
        await _db.SaveChangesAsync();

        dto.Id = e.Id;
        return Ok(dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] EmployeeDTO dto)
    {
        var e = await _db.Employees.FirstOrDefaultAsync(x => x.Id == id);
        if (e == null) return NotFound();

        e.FirstName = dto.FirstName;
        e.LastName = dto.LastName;
        e.Position = dto.Position;
        e.HireDate = dto.HireDate;
        e.IsActive = dto.IsActive;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await _db.Employees.FirstOrDefaultAsync(x => x.Id == id);
        if (e == null) return NotFound();

        _db.Employees.Remove(e);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}