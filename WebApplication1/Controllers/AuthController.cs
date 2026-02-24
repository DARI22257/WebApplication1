using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DB;
using WebApplication1.DTO;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly _1135ChubrikContext _db;
    private readonly IConfiguration _cfg;

    public AuthController(_1135ChubrikContext db, IConfiguration cfg)
    {
        _db = db;
        _cfg = cfg;
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
            return BadRequest("Username/password required");

        var cred = await _db.Credentials.FirstOrDefaultAsync(c => c.Username == req.Username);
        if (cred == null) return Unauthorized();


        if (cred.PasswordHash != req.Password)
            return Unauthorized();

        var employee = await _db.Employees.FirstOrDefaultAsync(e => e.Id == cred.Employeeid);
        if (employee == null) return Unauthorized();

        var token = CreateJwt(employee.Id, cred.Role);

        return Ok(new TokenResponse { Token = token, ExpiresIn = 3600 });
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<EmployeeRoleDTO>> Profile()
    {
        var employeeIdStr = User.FindFirstValue("EmployeeId");
        if (!int.TryParse(employeeIdStr, out var employeeId))
            return Unauthorized();

        var employee = await _db.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
        if (employee == null) return NotFound();

        var roleTitle = User.FindFirstValue(ClaimTypes.Role) ?? "User";

        return Ok(new EmployeeRoleDTO
        {
            EmployeeDto = new EmployeeDTO
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Position = employee.Position,
                HireDate = employee.HireDate,
                IsActive = employee.IsActive
            },
            RoleDto = new RoleDto { Id = 0, Title = roleTitle }
        });
    }

    private string CreateJwt(int employeeId, string role)
    {
        var key = _cfg["Jwt:Key"] ?? "DEV_SUPER_SECRET_KEY_1234567890";
        var issuer = _cfg["Jwt:Issuer"] ?? "WebApplication1";
        var audience = _cfg["Jwt:Audience"] ?? "WebApplication1Client";

        var claims = new List<Claim>
        {
            new("EmployeeId", employeeId.ToString()),
            new(ClaimTypes.Role, string.IsNullOrWhiteSpace(role) ? "User" : role)
        };

        var creds = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}