using Microsoft.AspNetCore.Mvc;
using CineReview.Api.DTOs;
using CineReview.Api.Services;
using System.Threading.Tasks;

namespace CineReview.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilmesController : ControllerBase
    {
        private readonly IMidiaService _midiaService;

        public FilmesController(IMidiaService midiaService)
        {
            _midiaService = midiaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? top)
        {
            var filmes = await _midiaService.GetFilmesAsync(top);
            return Ok(filmes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var filme = await _midiaService.GetFilmeByIdAsync(id);
            if (filme == null) return NotFound();
            return Ok(filme);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FilmeCreateDto dto)
        {
            var created = await _midiaService.CreateFilmeAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FilmeCreateDto dto)
        {
            
            return StatusCode(501, "Update não implementado no serviço (adicione método UpdateAsync na interface/serviço).");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            
            return StatusCode(501, "Delete não implementado no serviço (adicione método DeleteAsync na interface/serviço).");
        }

        [HttpGet("ranking")]
        public async Task<IActionResult> Ranking([FromQuery] int top = 10)
        {
            var ranked = await _midiaService.GetFilmesRankedAsync(top);
            return Ok(ranked);
        }
    }
}
