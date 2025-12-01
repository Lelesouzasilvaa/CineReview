namespace CineReview.Api.Models
{
    public abstract class Midia
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = "";
        public string? Descricao { get; set; }
        public double NotaMedia { get; set; }
    }
}