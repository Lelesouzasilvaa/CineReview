using CineReview.Api.Data;
using CineReview.Api.DTOs;
using CineReview.Api.Models;
using CineReview.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineReview.Api.Services.Implementations;

public class SerieService : ISerieService
{
    private readonly CineReviewContext _context;

    public SerieService(CineReviewContext context)
    {
        _context = context;
    }

    public async Task<SerieReadDto> CreateAsync(SerieCreateDto dto)
    {
        var serie = new Serie
        {
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            Temporadas = dto.Temporadas,
            Episodios = dto.Episodios,
            NotaMedia = 0
        };

        _context.Series.Add(serie);
        await _context.SaveChangesAsync();

        return MapToReadDto(serie);
    }

    public async Task<IEnumerable<SerieReadDto>> GetAllAsync(int? top = null)
    {
        var query = _context.Series.AsQueryable();

        if (top.HasValue && top.Value > 0)
            query = query.Take(top.Value);

        var list = await query.ToListAsync();
        return list.Select(MapToReadDto);
    }

    public async Task<SerieReadDto?> GetByIdAsync(int id)
    {
        var s = await _context.Series.FirstOrDefaultAsync(x => x.Id == id);
        if (s == null) return null;

        return MapToReadDto(s);
    }

    public async Task<SerieReadDto?> UpdateAsync(int id, SerieCreateDto dto)
    {
        var s = await _context.Series.FirstOrDefaultAsync(x => x.Id == id);
        if (s == null) return null;

        s.Titulo = dto.Titulo;
        s.Descricao = dto.Descricao;
        s.Temporadas = dto.Temporadas;
        s.Episodios = dto.Episodios;

        await _context.SaveChangesAsync();
        return MapToReadDto(s);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var s = await _context.Series.FirstOrDefaultAsync(x => x.Id == id);
        if (s == null) return false;

        _context.Series.Remove(s);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<SerieReadDto>> GetRankingAsync(int top)
    {
        var list = await _context.Series
            .OrderByDescending(s => s.NotaMedia)
            .Take(top)
            .ToListAsync();

        return list.Select(MapToReadDto);
    }

    // MAPPER
    private SerieReadDto MapToReadDto(Serie s)
    {
        return new SerieReadDto
        {
            Id = s.Id,
            Titulo = s.Titulo,
            Descricao = s.Descricao,
            Temporadas = s.Temporadas,
            Episodios = s.Episodios,
            NotaMedia = s.NotaMedia
        };
    }
}
