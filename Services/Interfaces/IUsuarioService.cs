using CineReview.Api.Models;
using System.Threading.Tasks;

namespace CineReview.Api.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario?> GetByIdAsync(int id);
    }
}