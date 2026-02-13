using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        public WebApplication db { get; set; }

        public AuthController(WebApplication db)
        {
            this.db = db;
        }


        [Authorize]
        [HttpPost("profile")]
        public async Task<ActionResult<EmployeeRoleDTO>> Profile()
        {
            var employeeId = int.Parse(User.FindFirst("EmployeeId")!.Value);

            var employee = await db.Employees.Include(e => e.Credentials).ThenInclude(c => c.Role)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null)
                return NotFound();

            return Ok(new EmployeeRoleDTO
            {
                EmployeeDto = new EmployeeDTO
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Position = employee.Position,
                },
                RoleDto = new RoleDto
                {
                    Title = employee.Credentials.Last().Role.Title
                },
            });
        }
    }
}

    
    

