using CineReview.Api.DTOs.Reviews;

namespace CineReview.Api.Services.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewResponseDto?> CriarAsync(ReviewCreateDto dto);
        Task<ReviewResponseDto?> BuscarPorIdAsync(int id);
        Task<ReviewResponseDto?> AtualizarAsync(int id, ReviewUpdateDto dto);
        Task<bool> ExcluirAsync(int id);

        // LISTAGENS
        Task<IEnumerable<ReviewResponseDto>> ListarPorUsuarioAsync(int usuarioId);
        Task<IEnumerable<ReviewResponseDto>> ListarPorMidiaAsync(int midiaId, string tipo);
    }
}
