using CineReview.Api.Data;
using CineReview.Api.DTOs;
using CineReview.Api.Models;
using CineReview.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineReview.Api.Services.Implementations
{
    public class FilmeService : IFilmeService
    {
        private readonly CineReviewContext _ctx;
        public FilmeService(CineReviewContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<FilmeReadDto> CreateAsync(FilmeCreateDto dto)
        {
            var filme = new Filme
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Diretor = dto.Diretor,
                Lancamento = dto.Lancamento,
            };

            _ctx.Filmes.Add(filme);
            await _ctx.SaveChangesAsync();

            return MapToReadDto(filme);
        }

        public async Task<IEnumerable<FilmeReadDto>> GetAllAsync(int? top = null)
        {
            var query = _ctx.Filmes.AsQueryable();

            if (top.HasValue && top.Value > 0)
                query = query.Take(top.Value);

            var list = await query.ToListAsync();
            return list.Select(MapToReadDto);
        }

        public async Task<FilmeReadDto?> GetByIdAsync(int id)
        {
            var f = await _ctx.Filmes.FirstOrDefaultAsync(x => x.Id == id);
            if (f == null) return null;
            return MapToReadDto(f);
        }

        public async Task<FilmeReadDto?> UpdateAsync(int id, FilmeCreateDto dto)
        {
            var f = await _ctx.Filmes.FirstOrDefaultAsync(x => x.Id == id);
            if (f == null) return null;

            f.Titulo = dto.Titulo;
            f.Descricao = dto.Descricao;
            f.Diretor = dto.Diretor;
            f.Lancamento = dto.Lancamento;

            await _ctx.SaveChangesAsync();
            return MapToReadDto(f);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var f = await _ctx.Filmes.FirstOrDefaultAsync(x => x.Id == id);
            if (f == null) return false;

            _ctx.Filmes.Remove(f);
            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<FilmeReadDto>> GetRankingAsync(int top)
        {
            var list = await _ctx.Filmes
                .OrderByDescending(f => f.NotaMedia)
                .Take(top)
                .ToListAsync();

            return list.Select(MapToReadDto);
        }

        // ---- helpers ----
        private FilmeReadDto MapToReadDto(Filme f)
        {
            return new FilmeReadDto
            {
                Id = f.Id,
                Titulo = f.Titulo,
                Descricao = f.Descricao,
                Diretor = f.Diretor,
                Lancamento = f.Lancamento,
                NotaMedia = f.NotaMedia
            };
        }
    }
}
