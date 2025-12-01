using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CineReview.Api.Data;
using CineReview.Api.Models;

namespace CineReview.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly CineReviewContext _context;
        private readonly IConfiguration _config;

        public AuthService(CineReviewContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // Valida se o usuário existe no banco
        public async Task<bool> ValidarCredenciaisAsync(string email, string senha)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);

            return usuario != null;
        }

        // Gera token JWT
        public string GerarToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtKey = _config["AppSettings:JwtKey"]
                         ?? throw new InvalidOperationException("AppSettings:JwtKey não configurado");
            var key = Encoding.ASCII.GetBytes(jwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
