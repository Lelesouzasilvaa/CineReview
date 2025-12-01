using CineReview.Api.DTOs.Reviews;

namespace CineReview.Api.Services.Interfaces
{
    public interface IAvaliacaoService
    {
        Task<ReviewResponseDto?> AvaliarAsync(ReviewCreateDto dto);
        Task<ReviewResponseDto?> BuscarAsync(int usuarioId, int midiaId);
        Task<IEnumerable<ReviewResponseDto>> ListarPorUsuarioAsync(int usuarioId);
        Task<IEnumerable<ReviewResponseDto>> ListarPorMidiaAsync(int midiaId);
    }
}
