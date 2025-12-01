namespace CineReview.Api.DTOs
{
    public class FilmeReadDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string? Diretor { get; set; }
        public DateTime? Lancamento { get; set; }
        public double NotaMedia { get; set; }
    }
}
