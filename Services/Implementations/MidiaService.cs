using CineReview.Api.Data;
using CineReview.Api.DTOs;
using CineReview.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CineReview.Api.Services.Implementations
{
    public class MidiaService : IMidiaService
    {
        private readonly CineReviewContext _context;

        public MidiaService(CineReviewContext context)
        {
            _context = context;
        }

        public async Task<FilmeReadDto> CreateFilmeAsync(FilmeCreateDto dto)
        {
            var filme = new Filme
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Diretor = dto.Diretor,
                Lancamento = dto.Lancamento,
                Genero = dto.Genero,
                DuracaoMinutos = dto.DuracaoMinutos,
                NotaMedia = 0
            };

            _context.Filmes.Add(filme);
            await _context.SaveChangesAsync();

            return MapToReadDto(filme);
        }

        public async Task<IEnumerable<FilmeReadDto>> GetFilmesAsync(int? top = null)
        {
            var query = _context.Filmes.AsQueryable();

            if (top.HasValue && top > 0)
                query = query.Take(top.Value);

            var list = await query.ToListAsync();
            return list.Select(MapToReadDto);
        }

        public async Task<FilmeReadDto?> GetFilmeByIdAsync(int id)
        {
            var filme = await _context.Filmes.FindAsync(id);
            if (filme == null) return null;

            return MapToReadDto(filme);
        }

        public async Task<FilmeReadDto?> UpdateFilmeAsync(int id, FilmeCreateDto dto)
        {
            var filme = await _context.Filmes.FindAsync(id);
            if (filme == null) return null;

            filme.Titulo = dto.Titulo;
            filme.Descricao = dto.Descricao;
            filme.Diretor = dto.Diretor;
            filme.Lancamento = dto.Lancamento;
            filme.Genero = dto.Genero;
            filme.DuracaoMinutos = dto.DuracaoMinutos;

            await _context.SaveChangesAsync();

            return MapToReadDto(filme);
        }

        public async Task<bool> DeleteFilmeAsync(int id)
        {
            var filme = await _context.Filmes.FindAsync(id);
            if (filme == null) return false;

            _context.Filmes.Remove(filme);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<FilmeReadDto>> GetFilmesRankedAsync(int top)
        {
            var list = await _context.Filmes
                .OrderByDescending(f => f.NotaMedia)
                .Take(top)
                .ToListAsync();

            return list.Select(MapToReadDto);
        }

        // ------------------------------
        // MAPPER
        // ------------------------------
        private FilmeReadDto MapToReadDto(Filme f)
        {
            return new FilmeReadDto
            {
                Id = f.Id,
                Titulo = f.Titulo,
                Descricao = f.Descricao,
                Diretor = f.Diretor,
                Genero = f.Genero,
                Lancamento = f.Lancamento,
                DuracaoMinutos = f.DuracaoMinutos,
                NotaMedia = f.NotaMedia
            };
        }
    }
}
