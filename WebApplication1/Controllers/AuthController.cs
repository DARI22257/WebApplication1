using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DB;
using WebApplication1.DTO;
using WebApplication1.Tools;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly _1135ChubrikContext _db;
    private readonly IConfiguration _cfg;

    public _1135ChubrikContext db { get; set; }
    public AuthController(_1135ChubrikContext db, IConfiguration cfg)
    {
        _db = db;
        _cfg = cfg;
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginRequest request)
    {
        var credential = await db.Credentials.Include(x => x.Role)
            .FirstOrDefaultAsync(c => c.Username == request.Username);
        if (credential == null)
            return Unauthorized();

        bool isValidPassword = BCrypt.Net.BCrypt.Verify(
            request.Password,
            credential.PasswordHash
        );

        if (!isValidPassword)
            return Unauthorized();

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, credential.Username),
            new Claim(ClaimTypes.Role, credential.Role),
            new Claim("EmployeeId", credential.Employeeid.ToString()),
        };

        var key = JwtSettings.GetSymmetricSecurityKey();
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expirensIn = 3600;

        var toker = new JwtSecurityToken(
            issuer: JwtSettings.ISSUER,
            audience: JwtSettings.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(expirensIn),
            signingCredentials: creds
        );
        return Ok(new TokenResponse()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(toker),
            ExpiresIn = expirensIn
        });
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

    [HttpPost]
    public ActionResult<string> Post(LoginRequest value)
    {
        if (value.Username == "1" && value.Password == "2")
        {
            List<Claim> claims = new();
            claims.Add(new Claim(ClaimTypes.PrimarySid, "asfas1315135315"));
            var jwt = new JwtSecurityToken(
                JwtSettings.ISSUER,
                JwtSettings.AUDIENCE, 
                claims, 
                null, 
                DateTime.UtcNow.AddMinutes(30),
                new SigningCredentials(JwtSettings.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Ok(token);
        }
        else
        {
            return Forbid();
        }
    }
}