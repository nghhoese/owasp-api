using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinesLogic.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration iconfiguration;

    public JwtService(IConfiguration iconfiguration)
    {
        this.iconfiguration = iconfiguration;
    }

    public string GetJwtToken()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(iconfiguration["JWT:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
          {
         new Claim(ClaimTypes.Name, "Administrator"),
         new Claim(ClaimTypes.Role, "Admin")
          }),
            Expires = DateTime.UtcNow.AddMinutes(10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}