using CineReview.Api.Data;
using CineReview.Api.DTOs;
using CineReview.Api.Models;
using CineReview.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineReview.Api.Services.Implementations
{
    public class FilmeService : IFilmeService
    {
        private readonly CineReviewContext _context;

        public FilmeService(CineReviewContext context)
        {
            _context = context;
        }

        public async Task<FilmeReadDto> CreateAsync(FilmeCreateDto dto)
        {
            var filme = new Filme
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Diretor = dto.Diretor,
                DuracaoMinutos = dto.DuracaoMinutos,
                Genero = dto.Genero,
                Lancamento = dto.Lancamento,
                NotaMedia = 0
            };

            _context.Filmes.Add(filme);
            await _context.SaveChangesAsync();

            return Map(filme);
        }

        public async Task<IEnumerable<FilmeReadDto>> GetAllAsync(int? top = null)
        {
            var query = _context.Filmes.AsQueryable();

            if (top.HasValue)
                query = query.Take(top.Value);

            var list = await query.ToListAsync();
            return list.Select(Map);
        }

        public async Task<FilmeReadDto?> GetByIdAsync(int id)
        {
            var f = await _context.Filmes.FirstOrDefaultAsync(x => x.Id == id);
            return f == null ? null : Map(f);
        }

        public async Task<FilmeReadDto?> UpdateAsync(int id, FilmeCreateDto dto)
        {
            var f = await _context.Filmes.FindAsync(id);
            if (f == null) return null;

            f.Titulo = dto.Titulo;
            f.Descricao = dto.Descricao;
            f.Diretor = dto.Diretor;
            f.Lancamento = dto.Lancamento;
            f.Genero = dto.Genero;
            f.DuracaoMinutos = dto.DuracaoMinutos;

            await _context.SaveChangesAsync();
            return Map(f);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var f = await _context.Filmes.FindAsync(id);
            if (f == null) return false;

            _context.Filmes.Remove(f);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<FilmeReadDto>> GetRankingAsync(int top)
        {
            var list = await _context.Filmes
                .OrderByDescending(f => f.NotaMedia)
                .Take(top)
                .ToListAsync();

            return list.Select(Map);
        }

        private FilmeReadDto Map(Filme f)
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
