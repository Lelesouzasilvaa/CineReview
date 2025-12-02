using CineReview.Api.Data;
using CineReview.Api.DTOs.Auth;
using CineReview.Api.Models;
using CineReview.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CineReview.Api.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly CineReviewContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(CineReviewContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Registro de usuário
        public async Task<UserReadDto> RegistrarAsync(RegisterDto dto)
        {
            // Verifica se o email já existe
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Email já cadastrado.");

            // Cria hash seguro da senha
            var senhaHash = GerarHashSeguro(dto.Senha);

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = senhaHash
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return new UserReadDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
            };
        }

        // Login
        public async Task<UserReadDto> LoginAsync(LoginDto dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario == null || !VerificarHashSeguro(dto.Senha, usuario.SenhaHash))
                throw new UnauthorizedAccessException("Email ou senha incorretos.");

            return new UserReadDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
            };
        }

        // Gera token JWT
        public string GerarToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Email, usuario.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // ========================
        // HASH SEGURO COM SALT
        // ========================
        private string GerarHashSeguro(string senha)
        {
            var salt = RandomNumberGenerator.GetBytes(16);
            using var derive = new Rfc2898DeriveBytes(senha, salt, 100_000, HashAlgorithmName.SHA256);
            var hash = derive.GetBytes(32);
            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        private bool VerificarHashSeguro(string senha, string senhaHash)
        {
            var parts = senhaHash.Split(':');
            var salt = Convert.FromBase64String(parts[0]);
            var hash = Convert.FromBase64String(parts[1]);

            using var derive = new Rfc2898DeriveBytes(senha, salt, 100_000, HashAlgorithmName.SHA256);
            var testHash = derive.GetBytes(32);

            return testHash.SequenceEqual(hash);
        }
    }
}
