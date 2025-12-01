using System.Threading.Tasks;

namespace CineReview.Api.Services
{
    public interface IAuthService
    {
        Task<bool> ValidarCredenciaisAsync(string email, string senha);
        string GerarToken(string email);
    }
}
