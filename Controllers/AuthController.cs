using Microsoft.AspNetCore.Mvc;

namespace CineReview.Api.Controllers
{
    public record LoginDTO(string Email, string Senha);

    public interface IAuthService
    {
        Task<bool> ValidarCredenciaisAsync(string email, string senha);
        string GerarToken(string email);
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (dto is null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var credenciaisValidas = await _authService.ValidarCredenciaisAsync(dto.Email, dto.Senha);

            if (!credenciaisValidas)
                return Unauthorized();

            var token = _authService.GerarToken(dto.Email);

            return Ok(new { Token = token });
        }
    }
}
