using CineReview.Api.DTOs.Auth;
using System.Threading.Tasks;

namespace CineReview.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserReadDto> RegistrarAsync(RegisterDto dto);
        Task<UserReadDto> LoginAsync(LoginDto dto);
        string GerarToken(CineReview.Api.Models.Usuario usuario);
    }
}
