using CineReview.Api.Data;
using CineReview.Api.DTOs.Auth;
using CineReview.Api.Models;
using CineReview.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CineReview.Api.Services.Implementations
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

        public async Task<UserReadDto> RegistrarAsync(RegisterDto dto)
        {
            if (await _context.Usuarios.AnyAsync(x => x.Email == dto.Email))
                throw new Exception("E-mail já cadastrado.");

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = dto.Senha  // senha "crua"
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return new UserReadDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Token = GerarToken(usuario)
            };
        }

        public async Task<UserReadDto> LoginAsync(LoginDto dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == dto.Email);

            // Comparação simples da senha em texto puro
            if (usuario == null || usuario.SenhaHash != dto.Senha)
                throw new UnauthorizedAccessException("Credenciais inválidas.");

            return new UserReadDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Token = GerarToken(usuario)
            };
        }

        public string GerarToken(Usuario usuario)
        {
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
