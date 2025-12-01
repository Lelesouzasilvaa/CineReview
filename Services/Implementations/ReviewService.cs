using CineReview.Api.Data;
using CineReview.Api.DTOs.Reviews;
using CineReview.Api.Models;
using CineReview.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineReview.Api.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly CineReviewContext _context;

        public ReviewService(CineReviewContext context)
        {
            _context = context;
        }

        public async Task<ReviewResponseDto?> CriarAsync(ReviewCreateDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);
            if (usuario == null) return null;

            Filme? filme = null;
            Serie? serie = null;
            string titulo = "";

            if (dto.Tipo == "Filme")
            {
                filme = await _context.Filmes.FindAsync(dto.MidiaId);
                if (filme == null) return null;
                titulo = filme.Titulo;
            }
            else if (dto.Tipo == "Serie")
            {
                serie = await _context.Series.FindAsync(dto.MidiaId);
                if (serie == null) return null;
                titulo = serie.Titulo;
            }

            var review = new Review
            {
                UsuarioId = dto.UsuarioId,
                Tipo = dto.Tipo,
                MidiaId = dto.MidiaId,
                Nota = dto.Nota,
                Comentario = dto.Comentario,
                Usuario = usuario,
                Filme = filme,
                Serie = serie
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            await RecalcularNotaMedia(dto.MidiaId, dto.Tipo);

            return new ReviewResponseDto
            {
                Id = review.Id,
                UsuarioId = usuario.Id,
                UsuarioNome = usuario.Nome,
                MidiaId = dto.MidiaId,
                MidiaTitulo = titulo,
                Tipo = dto.Tipo,
                Nota = dto.Nota,
                Comentario = dto.Comentario
            };
        }

        public async Task<IEnumerable<ReviewResponseDto>> ListarPorUsuarioAsync(int usuarioId)
        {
            var reviews = await _context.Reviews
                .Include(r => r.Usuario)
                .Where(r => r.UsuarioId == usuarioId)
                .ToListAsync();

            var lista = new List<ReviewResponseDto>();
            foreach (var r in reviews)
            {
                string titulo = "";
                if (r.Tipo == "Filme" && r.Filme != null) titulo = r.Filme.Titulo;
                if (r.Tipo == "Serie" && r.Serie != null) titulo = r.Serie.Titulo;

                lista.Add(new ReviewResponseDto
                {
                    Id = r.Id,
                    UsuarioId = r.UsuarioId,
                    UsuarioNome = r.Usuario?.Nome ?? "",
                    MidiaId = r.MidiaId,
                    MidiaTitulo = titulo,
                    Tipo = r.Tipo,
                    Nota = r.Nota,
                    Comentario = r.Comentario
                });
            }
            return lista;
        }

        public async Task<IEnumerable<ReviewResponseDto>> ListarPorMidiaAsync(int midiaId, string tipo)
        {
            var reviews = await _context.Reviews
                .Include(r => r.Usuario)
                .Where(r => r.MidiaId == midiaId && r.Tipo == tipo)
                .ToListAsync();

            var lista = new List<ReviewResponseDto>();
            foreach (var r in reviews)
            {
                string titulo = tipo == "Filme" ? r.Filme?.Titulo ?? "" : r.Serie?.Titulo ?? "";

                lista.Add(new ReviewResponseDto
                {
                    Id = r.Id,
                    UsuarioId = r.UsuarioId,
                    UsuarioNome = r.Usuario?.Nome ?? "",
                    MidiaId = r.MidiaId,
                    MidiaTitulo = titulo,
                    Tipo = r.Tipo,
                    Nota = r.Nota,
                    Comentario = r.Comentario
                });
            }
            return lista;
        }

        private async Task RecalcularNotaMedia(int midiaId, string tipo)
        {
            double media = 0;
            if (tipo == "Filme")
            {
                var avaliacoes = await _context.Reviews.Where(r => r.MidiaId == midiaId && r.Tipo == "Filme").ToListAsync();
                var filme = await _context.Filmes.FindAsync(midiaId);
                if (filme != null) filme.NotaMedia = avaliacoes.Average(r => r.Nota);
            }
            else if (tipo == "Serie")
            {
                var avaliacoes = await _context.Reviews.Where(r => r.MidiaId == midiaId && r.Tipo == "Serie").ToListAsync();
                var serie = await _context.Series.FindAsync(midiaId);
                if (serie != null) serie.NotaMedia = avaliacoes.Average(r => r.Nota);
            }
            await _context.SaveChangesAsync();
        }
    }
}
