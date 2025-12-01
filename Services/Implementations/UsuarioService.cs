using CineReview.Api.Data;
using CineReview.Api.Models;
using CineReview.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineReview.Api.Services.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        private readonly CineReviewContext _context;

        public UsuarioService(CineReviewContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }
    }
}
