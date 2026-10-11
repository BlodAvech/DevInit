using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Models.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace backend.Extensions;

public class JWTService
{
    private readonly string secretKey;

    public JWTService(IConfiguration configuration)
    {
        secretKey = configuration.GetConnectionString("DefaultConnection")! ;
    }

    public string GenerateToken(TokenDTO tokenDTO)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
			[
				new Claim(ClaimTypes.NameIdentifier , tokenDTO.Id),
                new Claim(ClaimTypes.Name , tokenDTO.Name ?? ""),
            ]),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public void CreateTokenCookie(HttpContext context , string token) => 
        context.Response.Cookies.Append("token" , token , new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Secure = true,
            Expires = DateTime.UtcNow.AddHours(1),
        });
    
}
