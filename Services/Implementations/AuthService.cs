using CineReview.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

public class AuthService : IAuthServices
{
    private readonly CineReviewContext _context;
    private readonly IConfiguration _config;

    public AuthService(CineReviewContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<string?> Login(string email, string senha)
    {
        // Busca apenas por email (coluna Senha não existe no esquema)
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email);

        if (usuario == null)
            return null;

        // Verifica senha: se o valor em banco for "salt:hash" usa PBKDF2,
        // caso contrário aceita igualdade direta (compatibilidade com dados antigos).
        if (!VerifyPassword(usuario.SenhaHash, senha))
            return null;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: new[]
            {
                new Claim("id", usuario.Id.ToString()),
                new Claim("email", usuario.Email)
            },
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Suporta dois formatos:
    // - "saltBase64:hashBase64" -> PBKDF2-SHA256 (recomendado)
    // - "plain" -> comparação direta (compatibilidade)
    private static bool VerifyPassword(string stored, string provided)
    {
        if (string.IsNullOrEmpty(stored))
            return false;

        if (stored.Contains(':'))
        {
            var parts = stored.Split(':', 2);
            try
            {
                var salt = Convert.FromBase64String(parts[0]);
                var hash = Convert.FromBase64String(parts[1]);

                using var derive = new Rfc2898DeriveBytes(provided, salt, 100_000, HashAlgorithmName.SHA256);
                var testHash = derive.GetBytes(hash.Length);
                return CryptographicOperations.FixedTimeEquals(testHash, hash);
            }
            catch
            {
                return false;
            }
        }

        // Compatibilidade com dados antigos (senha em texto simples)
        return stored == provided;
    }
}
