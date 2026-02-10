using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
    if (value.username == "1" && value.Password == "2")
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

[Login]
[HttpPost("Check")]
public ActionResult<string> Post()
{
    return Ok("хахах");
}

    
    

