namespace CineReview.Api.Models
{
    public class Filme : Midia
    {
        public string? Diretor { get; set; }
        public DateTime? Lancamento { get; set; }
    }
}