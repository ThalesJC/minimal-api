using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using MinimalApi.Domain.Entities;

namespace MinimalApi.Domain.Utilities;

public static class Auth
{
  public static string GenerateToken(string key, Admin admin)
  {
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    var claims = new List<Claim>
         {
            new("Email", admin.Email),
            new("Role", admin.Role),
            new(ClaimTypes.Role, admin.Role)
        };
    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddHours(1),
        signingCredentials: creds
      );
    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);


    return tokenString;
  }
}

