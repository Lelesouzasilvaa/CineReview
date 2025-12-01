using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthServices _auth;

    public AuthController(IAuthServices auth)
    {
        _auth = auth;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        var token = await _auth.Login(dto.Email, dto.Senha);

        if (token == null)
            return Unauthorized("Email ou senha inválidos");

        return Ok(new { token });
    }
}
