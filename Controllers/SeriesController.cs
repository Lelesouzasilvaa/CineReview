using CineReview.Api.DTOs;
using CineReview.Api.Services;
using CineReview.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineReview.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeriesController : ControllerBase
{
    private readonly ISerieService _service;

    public SeriesController(ISerieService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(SerieCreateDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? top = null)
    {
        return Ok(await _service.GetAllAsync(top));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SerieCreateDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result == null ? NotFound() : Ok(result);
    }


    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }

    [HttpGet("ranking/{top}")]
    public async Task<IActionResult> Ranking(int top)
    {
        return Ok(await _service.GetRankingAsync(top));
    }
}
