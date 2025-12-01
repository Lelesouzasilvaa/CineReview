using CineReview.Api.DTOs.Reviews;

namespace CineReview.Api.Services.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewResponseDto?> CriarAsync(ReviewCreateDto dto);
        Task<IEnumerable<ReviewResponseDto>> ListarPorUsuarioAsync(int usuarioId);
        Task<IEnumerable<ReviewResponseDto>> ListarPorMidiaAsync(int midiaId, string tipo);
    }
}
