using CineReview.Api.Models;
using CineReview.Api.Data;
public static class SeedData
{
    public static void Initialize(CineReviewContext context)
    {
        if (!context.Filmes.Any())
        {
            context.Filmes.AddRange(
                new Filme { Titulo = "Inception", Descricao = "Sonhos e realidade", NotaMedia = 9.2 },
                new Filme { Titulo = "Interestelar", Descricao = "Exploração espacial", NotaMedia = 9.5 }
            );
            context.SaveChanges();
        }
    }
}
