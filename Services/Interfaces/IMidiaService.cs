using CineReview.Api.DTOs;

namespace CineReview.Api.Services;

public interface IMidiaService
{
    Task<FilmeReadDto> CreateFilmeAsync(FilmeCreateDto dto);
    Task<IEnumerable<FilmeReadDto>> GetFilmesAsync(int? top = null);
    Task<IEnumerable<FilmeReadDto>> GetFilmesRankedAsync(int top);
    Task<FilmeReadDto?> GetFilmeByIdAsync(int id);
}
