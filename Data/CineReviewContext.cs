using Microsoft.EntityFrameworkCore;
using CineReview.Api.Models;

namespace CineReview.Api.Data 
{
    public class CineReviewContext : DbContext
    {
        public CineReviewContext(DbContextOptions<CineReviewContext> options)
            : base(options)
        {
        }

        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Serie> Series { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Avaliacoes)
                .WithOne()
                .HasForeignKey(a => a.UsuarioId);
        }
    }
}
