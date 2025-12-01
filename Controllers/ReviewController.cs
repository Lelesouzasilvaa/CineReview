using CineReview.Api.DTOs.Reviews;
using CineReview.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CineReview.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // POST: api/Reviews
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] ReviewCreateDto dto)
        {
            var result = await _reviewService.CriarAsync(dto);
            if (result == null) return BadRequest("Usuário ou mídia não encontrada.");

            // Retorna 201 Created com a localização do novo recurso
            return CreatedAtAction(nameof(BuscarPorId), new { id = result.Id }, result);
        }

        // GET: api/Reviews/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var review = await _reviewService.BuscarPorIdAsync(id);
            if (review == null) return NotFound("Review não encontrada.");
            return Ok(review);
        }

        // GET: api/Reviews/usuario/{usuarioId}
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> ListarPorUsuario(int usuarioId)
        {
            var lista = await _reviewService.ListarPorUsuarioAsync(usuarioId);
            return Ok(lista);
        }

        // GET: api/Reviews/midia/{tipo}/{midiaId}
        [HttpGet("midia/{tipo}/{midiaId}")]
        public async Task<IActionResult> ListarPorMidia(string tipo, int midiaId)
        {
            var lista = await _reviewService.ListarPorMidiaAsync(midiaId, tipo);
            return Ok(lista);
        }

        // PUT: api/Reviews/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] ReviewUpdateDto dto)
        {
            var result = await _reviewService.AtualizarAsync(id, dto);
            if (result == null) return NotFound("Review não encontrada.");
            return Ok(result);
        }

        // DELETE: api/Reviews/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var sucesso = await _reviewService.ExcluirAsync(id);
            if (!sucesso) return NotFound("Review não encontrada.");

            return NoContent(); // Retorna 204 No Content, padrão para exclusão bem-sucedida
        }
    }
}