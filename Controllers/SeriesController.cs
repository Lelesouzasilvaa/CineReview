using Microsoft.AspNetCore.Mvc;
using CineReview.Api.Data;
using CineReview.Api.Models;

[ApiController]
[Route("api/[controller]")]
public class SeriesController : ControllerBase
{
    private readonly CineReviewContext _context;

    public SeriesController(CineReviewContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetSeries()
    {
        return Ok(_context.Series.ToList());
    }

    [HttpPost]
    public IActionResult CriarSerie([FromBody] Serie serie)
    {
        _context.Series.Add(serie);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetSeries), new { id = serie.Id }, serie);
    }
}
