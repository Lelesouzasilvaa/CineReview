using CineReview.Api.Data;
using CineReview.Api.DTOs.Reviews;
using CineReview.Api.Models;
using CineReview.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineReview.Api.Services.Implementations
{
    public class AvaliacaoService : IAvaliacaoService
    {
        private readonly CineReviewContext _context;

        public AvaliacaoService(CineReviewContext context)
        {
            _context = context;
        }

        public async Task<ReviewResponseDto?> AvaliarAsync(ReviewCreateDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);
            if (usuario == null) return null;

            // Busca a mídia correta
            Midia? midia = null;
            if (dto.Tipo.ToLower() == "filme")
                midia = await _context.Filmes.FindAsync(dto.MidiaId);
            else if (dto.Tipo.ToLower() == "serie")
                midia = await _context.Series.FindAsync(dto.MidiaId);

            if (midia == null) return null;

            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.UsuarioId == dto.UsuarioId && r.MidiaId == dto.MidiaId);

            if (review == null)
            {
                review = new Review
                {
                    UsuarioId = dto.UsuarioId,
                    MidiaId = dto.MidiaId,
                    Tipo = dto.Tipo,
                    Nota = dto.Nota,
                    Comentario = dto.Comentario
                };
                _context.Reviews.Add(review);
            }
            else
            {
                review.Nota = dto.Nota;
                review.Comentario = dto.Comentario;
            }

            await _context.SaveChangesAsync();

            // Atualiza a nota média da mídia
            await RecalcularNotaMedia(dto.MidiaId, dto.Tipo);

            return MapToResponse(review, usuario, midia);
        }

        public async Task<ReviewResponseDto?> BuscarAsync(int usuarioId, int midiaId)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.UsuarioId == usuarioId && r.MidiaId == midiaId);

            if (review == null) return null;

            var usuario = await _context.Usuarios.FindAsync(usuarioId);

            Midia? midia = null;
            if (review.Tipo.ToLower() == "filme")
                midia = await _context.Filmes.FindAsync(midiaId);
            else if (review.Tipo.ToLower() == "serie")
                midia = await _context.Series.FindAsync(midiaId);

            if (usuario == null || midia == null) return null;

            return MapToResponse(review, usuario, midia);
        }

        public async Task<IEnumerable<ReviewResponseDto>> ListarPorUsuarioAsync(int usuarioId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.UsuarioId == usuarioId)
                .ToListAsync();

            var result = new List<ReviewResponseDto>();

            foreach (var r in reviews)
            {
                var usuario = await _context.Usuarios.FindAsync(r.UsuarioId);

                Midia? midia = null;
                if (r.Tipo.ToLower() == "filme")
                    midia = await _context.Filmes.FindAsync(r.MidiaId);
                else if (r.Tipo.ToLower() == "serie")
                    midia = await _context.Series.FindAsync(r.MidiaId);

                if (usuario != null && midia != null)
                    result.Add(MapToResponse(r, usuario, midia));
            }

            return result;
        }

        public async Task<IEnumerable<ReviewResponseDto>> ListarPorMidiaAsync(int midiaId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.MidiaId == midiaId)
                .ToListAsync();

            var result = new List<ReviewResponseDto>();

            foreach (var r in reviews)
            {
                var usuario = await _context.Usuarios.FindAsync(r.UsuarioId);

                Midia? midia = null;
                if (r.Tipo.ToLower() == "filme")
                    midia = await _context.Filmes.FindAsync(r.MidiaId);
                else if (r.Tipo.ToLower() == "serie")
                    midia = await _context.Series.FindAsync(r.MidiaId);

                if (usuario != null && midia != null)
                    result.Add(MapToResponse(r, usuario, midia));
            }

            return result;
        }

        // Atualiza a nota média da mídia
        private async Task RecalcularNotaMedia(int midiaId, string tipo)
        {
            List<Review> avaliacoes;

            if (tipo.ToLower() == "filme")
                avaliacoes = await _context.Reviews
                    .Where(r => r.MidiaId == midiaId && r.Tipo.ToLower() == "filme")
                    .ToListAsync();
            else
                avaliacoes = await _context.Reviews
                    .Where(r => r.MidiaId == midiaId && r.Tipo.ToLower() == "serie")
                    .ToListAsync();

            Midia? midia = null;
            if (tipo.ToLower() == "filme")
                midia = await _context.Filmes.FindAsync(midiaId);
            else
                midia = await _context.Series.FindAsync(midiaId);

            if (midia != null && avaliacoes.Count > 0)
            {
                midia.NotaMedia = avaliacoes.Average(a => a.Nota);
                await _context.SaveChangesAsync();
            }
        }

        private ReviewResponseDto MapToResponse(Review review, Usuario usuario, Midia midia)
        {
            return new ReviewResponseDto
            {
                Id = review.Id,
                UsuarioId = usuario.Id,
                UsuarioNome = usuario.Nome,
                MidiaId = midia.Id,
                MidiaTitulo = midia.Titulo,
                Tipo = review.Tipo, // ou midia is Filme ? "Filme" : "Serie"
                Nota = review.Nota,
                Comentario = review.Comentario
            };
        }

    }
}
