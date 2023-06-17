using Microsoft.IdentityModel.Tokens;
using Noticias.Models;
using Shop;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Noticias.Services
{
    public class TokenService
    {
        private static Byte[]? Key { get; set; }
        public TokenService()
        {
            Key = Encoding.ASCII.GetBytes(Settings.Secret);
        }
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email,user.Email.ToString()),
                    new Claim(ClaimTypes.Role,user.Perfil.ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public User? VerifyToken(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            JwtSecurityToken readToken = tokenHandler.ReadJwtToken(token);
            string? email = readToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            string? perfil = readToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
            if (perfil.IsNullOrEmpty() || email.IsNullOrEmpty()) throw new ExceptionService("Token inválido",HttpStatusCode.Unauthorized);
            return new UserService().GetUsers().FirstOrDefault(c => c.Email == email);
        }

        public string RefreshToken(string tokenOld)
        {
            User user = VerifyToken(tokenOld);
            return GenerateToken(user);
        }
    }
}
