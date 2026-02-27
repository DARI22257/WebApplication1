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
                IsActive = e.IsActive,
                
            })
            .ToListAsync();

        return Ok(list);
    }

    [HttpPost]
    public  async Task<ActionResult> AddEmployee([FromBody]CreateEmployeeDto employeeCred)
    {
        
        if (await _db.Credentials.FirstOrDefaultAsync(x => x.Username == employeeCred.ProfileDto.Username) != null)
            return BadRequest("Username already exists");
        
        Employee employee = new Employee
        {
            FirstName = employeeCred.EmployeeDto.FirstName,
            LastName = employeeCred.EmployeeDto.LastName,
            Position = employeeCred.EmployeeDto.Position,
            HireDate = employeeCred.EmployeeDto.HireDate,
            IsActive = employeeCred.EmployeeDto.IsActive,
        }; 
       _db.Employees.Add(employee);
        await _db.SaveChangesAsync();
        
        Credential credential = new Credential
        {
            Username = employeeCred.ProfileDto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(employeeCred.ProfileDto.PasswordHash),
            RoleId = employeeCred.ProfileDto.RoleId,
            EmployeeId = employee.Id,
        };
        _db.Credentials.Add(credential);
        await db.SaveChangesAsync();
        
        CredentialDTO credentialDto = new CredentialDTO()
        {
            Id =  credential.Id,
            Username = credential.Username,
            PasswordHash = credential.PasswordHash,
            RoleId = credential.RoleId,
            EmployeeId = credential.EmployeeId,
        };
        
        return Created($"", credentialDto);
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