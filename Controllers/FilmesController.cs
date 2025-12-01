using CineReview.Api.DTOs;
using CineReview.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CineReview.Api.Controllers
{
    [ApiController]
    [Route("api/filmes")]
    public class FilmesController : ControllerBase
    {
        private readonly IFilmeService _filmeService;

        public FilmesController(IFilmeService filmeService)
        {
            _filmeService = filmeService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(FilmeCreateDto dto)
        {
            var result = await _filmeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? top)
        {
            return Ok(await _filmeService.GetAllAsync(top));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _filmeService.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, FilmeCreateDto dto)
        {
            var result = await _filmeService.UpdateAsync(id, dto);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _filmeService.DeleteAsync(id)
                ? NoContent()
                : NotFound();
        }

        [HttpGet("ranking/{top}")]
        public async Task<IActionResult> Ranking(int top)
        {
            return Ok(await _filmeService.GetRankingAsync(top));
        }
    }
}
