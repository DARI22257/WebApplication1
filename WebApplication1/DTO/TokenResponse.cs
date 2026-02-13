namespace WebApplication1.DTO;

public class TokenResponse
{
    public string Token { get; set; } = "";
    public int ExpiresIn { get; set; }
}