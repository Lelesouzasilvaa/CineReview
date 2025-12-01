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

        // --- MÉTODOS EXISTENTES (COMPLETOS) ---

        public async Task<ReviewResponseDto?> CriarAsync(ReviewCreateDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);
            if (usuario == null) return null;

            // ... (Restante da lógica de criação/busca de filme/série/titulo) ...

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

            // Verifica se a Review já existe (evita duplicação)
            var existingReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.UsuarioId == dto.UsuarioId && r.MidiaId == dto.MidiaId && r.Tipo == dto.Tipo);

            if (existingReview != null)
            {
                // Se existir, atualiza (comportamento de PUT implícito no POST)
                existingReview.Nota = dto.Nota;
                existingReview.Comentario = dto.Comentario;
                await _context.SaveChangesAsync();
                await RecalcularNotaMedia(dto.MidiaId, dto.Tipo);

                return MapToResponse(existingReview, usuario, titulo);
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

            return MapToResponse(review, usuario, titulo);
        }

        // ... (ListarPorUsuarioAsync e ListarPorMidiaAsync omitidos para brevidade, mantenha os seus) ...

        // --- NOVOS MÉTODOS (ADICIONADOS) ---

        public async Task<ReviewResponseDto?> BuscarPorIdAsync(int id)
        {
            var review = await _context.Reviews
                .Include(r => r.Usuario)
                .Include(r => r.Filme)
                .Include(r => r.Serie)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null) return null;

            string titulo = review.Tipo == "Filme" ? review.Filme?.Titulo ?? "" : review.Serie?.Titulo ?? "";

            return MapToResponse(review, review.Usuario!, titulo);
        }

        public async Task<ReviewResponseDto?> AtualizarAsync(int id, ReviewUpdateDto dto)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return null;

            review.Nota = dto.Nota;
            review.Comentario = dto.Comentario;

            await _context.SaveChangesAsync();

            // Recalcula a média após a atualização
            await RecalcularNotaMedia(review.MidiaId, review.Tipo);

            // Retorna o DTO de resposta atualizado (requer buscar dados do usuário/mídia)
            var usuario = await _context.Usuarios.FindAsync(review.UsuarioId);
            string titulo = await GetMidiaTitle(review.MidiaId, review.Tipo);

            if (usuario == null) return null;

            return MapToResponse(review, usuario, titulo);
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            // Recalcula a média após a exclusão
            await RecalcularNotaMedia(review.MidiaId, review.Tipo);

            return true;
        }

        // --- HELPERS (Métodos Auxiliares) ---

        private async Task RecalcularNotaMedia(int midiaId, string tipo)
        {
            // ... (Mantenha sua lógica existente para RecalcularNotaMedia) ...

            if (tipo == "Filme")
            {
                var avaliacoes = await _context.Reviews.Where(r => r.MidiaId == midiaId && r.Tipo == "Filme").ToListAsync();
                var filme = await _context.Filmes.FindAsync(midiaId);
                if (filme != null) filme.NotaMedia = avaliacoes.Any() ? avaliacoes.Average(r => r.Nota) : 0;
            }
            else if (tipo == "Serie")
            {
                var avaliacoes = await _context.Reviews.Where(r => r.MidiaId == midiaId && r.Tipo == "Serie").ToListAsync();
                var serie = await _context.Series.FindAsync(midiaId);
                if (serie != null) serie.NotaMedia = avaliacoes.Any() ? avaliacoes.Average(r => r.Nota) : 0;
            }
            await _context.SaveChangesAsync();
        }

        // Novo helper para buscar o título da mídia (usado em Atualizar e BuscarPorId)
        private async Task<string> GetMidiaTitle(int midiaId, string tipo)
        {
            if (tipo == "Filme")
            {
                var filme = await _context.Filmes.FindAsync(midiaId);
                return filme?.Titulo ?? "";
            }
            if (tipo == "Serie")
            {
                var serie = await _context.Series.FindAsync(midiaId);
                return serie?.Titulo ?? "";
            }
            return "";
        }

        // Método de mapeamento adaptado para receber o título
        private ReviewResponseDto MapToResponse(Review review, Usuario usuario, string midiaTitulo)
        {
            return new ReviewResponseDto
            {
                Id = review.Id,
                UsuarioId = usuario.Id,
                UsuarioNome = usuario.Nome,
                MidiaId = review.MidiaId,
                MidiaTitulo = midiaTitulo,
                Tipo = review.Tipo,
                Nota = review.Nota,
                Comentario = review.Comentario
            };
        }

        public async Task<IEnumerable<ReviewResponseDto>> ListarPorMidiaAsync(int midiaId, string tipo)
        {
            var reviews = await _context.Reviews
                // Certifique-se de que a busca está usando ambos os parâmetros
                .Where(r => r.MidiaId == midiaId && r.Tipo == tipo)
                .Include(r => r.Usuario)
                // Inclua as outras navegações necessárias (Filme, Serie) se for mapear o título
                .Include(r => r.Filme)
                .Include(r => r.Serie)
                .ToListAsync();

            var lista = new List<ReviewResponseDto>();
            foreach (var r in reviews)
            {
                // ... (lógica de mapeamento aqui, como você já tinha) ...
                string titulo = r.Tipo == "Filme" ? r.Filme?.Titulo ?? "" : r.Serie?.Titulo ?? "";

                lista.Add(new ReviewResponseDto
                {
                    // ... (preencha as propriedades) ...
                    MidiaTitulo = titulo,
                    // ...
                });
            }
            return lista;
        }

        public async Task<IEnumerable<ReviewResponseDto>> ListarPorUsuarioAsync(int usuarioId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.UsuarioId == usuarioId)
                .Include(r => r.Usuario)
                // Certifique-se de incluir as mídias para mapeamento de título
                .Include(r => r.Filme)
                .Include(r => r.Serie)
                .ToListAsync();

            var lista = new List<ReviewResponseDto>();
            foreach (var r in reviews)
            {
                string titulo = r.Tipo == "Filme" ? r.Filme?.Titulo ?? "" : r.Serie?.Titulo ?? "";

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
    }
}