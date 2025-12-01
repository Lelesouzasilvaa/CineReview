namespace CineReview.Api.Services.Interfaces;
using CineReview.Api.DTOs;

public interface ISerieService
{
    Task<SerieReadDto> CreateAsync(SerieCreateDto dto);
    Task<IEnumerable<SerieReadDto>> GetAllAsync(int? top = null);
    Task<SerieReadDto?> GetByIdAsync(int id);
    Task<SerieReadDto?> UpdateAsync(int id, SerieCreateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<SerieReadDto>> GetRankingAsync(int top);
}
