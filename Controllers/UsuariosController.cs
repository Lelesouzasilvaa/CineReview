using Microsoft.AspNetCore.Mvc;
using CineReview.Api.Data;
using CineReview.Api.Models;
using CineReview.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CineReview.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly CineReviewContext _context;

        public UsuariosController(CineReviewContext context)
        {
            _context = context;
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<IActionResult> CriarUsuario([FromBody] RegistroDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verifica se o email já existe
            var existe = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email);
            if (existe)
                return Conflict("Email já cadastrado.");

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = dto.Senha // para produção, use hash!
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CriarUsuario), new { id = usuario.Id }, usuario);
        }
    }
}
