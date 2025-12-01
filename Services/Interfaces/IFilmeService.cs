using CineReview.Api.DTOs;

namespace CineReview.Api.Services.Interfaces
{
    public interface IFilmeService
    {
        Task<FilmeReadDto> CreateAsync(FilmeCreateDto dto);
        Task<IEnumerable<FilmeReadDto>> GetAllAsync(int? top = null);
        Task<FilmeReadDto?> GetByIdAsync(int id);
        Task<FilmeReadDto?> UpdateAsync(int id, FilmeCreateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<FilmeReadDto>> GetRankingAsync(int top);
    }

}
