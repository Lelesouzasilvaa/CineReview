using CineReview.Api.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public ICollection<Avaliacao>? Avaliacoes { get; set; }
}
