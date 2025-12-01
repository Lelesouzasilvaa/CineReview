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

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] ReviewCreateDto dto)
        {
            var result = await _reviewService.CriarAsync(dto);
            if (result == null) return BadRequest("Usuário ou mídia não encontrada.");
            return Ok(result);
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> ListarPorUsuario(int usuarioId)
        {
            var lista = await _reviewService.ListarPorUsuarioAsync(usuarioId);
            return Ok(lista);
        }

        [HttpGet("midia/{tipo}/{midiaId}")]
        public async Task<IActionResult> ListarPorMidia(string tipo, int midiaId)
        {
            var lista = await _reviewService.ListarPorMidiaAsync(midiaId, tipo);
            return Ok(lista);
        }
    }
}
