using CineReview.Api.Data;
using CineReview.Api.DTOs.Auth;
using CineReview.Api.Models;
using CineReview.Api.Services;
using CineReview.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace CineReview.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly CineReviewContext _context;
        private readonly IAuthService _authService;

        public AuthController(CineReviewContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var exists = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email);
            if (exists)
                return Conflict("Email já cadastrado.");

            // Hash da senha
            var salt = RandomNumberGenerator.GetBytes(16);
            using var derive = new Rfc2898DeriveBytes(dto.Senha, salt, 100_000, HashAlgorithmName.SHA256);
            var hash = derive.GetBytes(32);
            var senhaHash = $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = senhaHash
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Register), new { id = usuario.Id }, new { usuario.Id, usuario.Nome, usuario.Email });
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var user = await _authService.LoginAsync(dto); // chama LoginAsync passando o DTO
                return Ok(new { Token = user.Token });        // retorna o token gerado
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Email ou senha incorretos.");
            }
        }

    }
}
