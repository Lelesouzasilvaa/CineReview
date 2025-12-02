using CineReview.Api.DTOs.Auth;
using CineReview.Api.Models;
using CineReview.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CineReview.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _authService.RegistrarAsync(dto);

                // Gera token já no registro
                var token = _authService.GerarToken(new Usuario
                {
                    Id = user.Id,
                    Nome = user.Nome,
                    Email = user.Email
                });

                return Ok(new
                {
                    mensagem = "Usuário registrado com sucesso!",
                    usuario = user,
                    token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _authService.LoginAsync(dto);

                var token = _authService.GerarToken(new Usuario
                {
                    Id = user.Id,
                    Nome = user.Nome,
                    Email = user.Email
                });

                return Ok(new
                {
                    mensagem = "Login realizado com sucesso!",
                    usuario = user,
                    token
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Email ou senha incorretos.");
            }
        }
    }
}
