using CineReview.Api.Data;
using CineReview.Api.DTOs;
using CineReview.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CineReview.Api.Services.Implementations;

public class MidiaService : IMidiaService
{
    private readonly CineReviewContext _context;

    public MidiaService(CineReviewContext context)
    {
        _context = context;
    }


    // 1) Criar Filme

    public async Task<FilmeReadDto> CreateFilmeAsync(FilmeCreateDto dto)
    {
        var filme = new Filme
        {
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            Diretor = dto.Diretor,
            Lancamento = dto.Lancamento,
            NotaMedia = 0 // começa com 0 antes das avaliações
        };

        _context.Filmes.Add(filme);
        await _context.SaveChangesAsync();

        return new FilmeReadDto
        {
            Id = filme.Id,
            Titulo = filme.Titulo,
            Descricao = filme.Descricao,
            Diretor = filme.Diretor,
            Lancamento = filme.Lancamento,
            NotaMedia = filme.NotaMedia
        };
    }

    // 2) Listar todos os filmes

    public async Task<IEnumerable<FilmeReadDto>> GetFilmesAsync(int? top = null)
    {
        var query = _context.Filmes
            .OrderByDescending(f => f.Id)
            .AsQueryable();

        if (top.HasValue)
            query = query.Take(top.Value);

        var filmes = await query.ToListAsync();

        return filmes.Select(f => new FilmeReadDto
        {
            Id = f.Id,
            Titulo = f.Titulo,
            Descricao = f.Descricao,
            Diretor = f.Diretor,
            Lancamento = f.Lancamento,
            NotaMedia = f.NotaMedia
        });
    }

    // 3) Listar filmes mais bem avaliados

    public async Task<IEnumerable<FilmeReadDto>> GetFilmesRankedAsync(int top)
    {
        var filmes = await _context.Filmes
            .OrderByDescending(f => f.NotaMedia)
            .Take(top)
            .ToListAsync();

        return filmes.Select(f => new FilmeReadDto
        {
            Id = f.Id,
            Titulo = f.Titulo,
            Descricao = f.Descricao,
            Diretor = f.Diretor,
            Lancamento = f.Lancamento,
            NotaMedia = f.NotaMedia
        });
    }


    // 4) Buscar filme por ID

    public async Task<FilmeReadDto?> GetFilmeByIdAsync(int id)
    {
        var filme = await _context.Filmes.FindAsync(id);

        if (filme == null)
            return null;

        return new FilmeReadDto
        {
            Id = filme.Id,
            Titulo = filme.Titulo,
            Descricao = filme.Descricao,
            Diretor = filme.Diretor,
            Lancamento = filme.Lancamento,
            NotaMedia = filme.NotaMedia
        };
    }
}
