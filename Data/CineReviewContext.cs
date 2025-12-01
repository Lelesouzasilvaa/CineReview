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
        public DbSet<Midia> Midias { get; set; }

        public DbSet<Review> Reviews { get; set; }  // ← Review é a tabela de avaliações


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Reviews)
                .WithOne(r => r.Usuario)
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Midia>()
                .HasMany(m => m.Reviews)
                .WithOne(r => r.Midia)
                .HasForeignKey(r => r.MidiaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
