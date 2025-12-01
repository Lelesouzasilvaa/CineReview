using CineReview.Api.Services.Implementations;
using System.Collections.Generic;

namespace CineReview.Api.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;

        // Relação com reviews (filmes ou séries)
        public ICollection<Review>? Reviews { get; set; }
    }
}
