namespace CineReview.Api.DTOs
{
    public class FilmeCreateDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string? Diretor { get; set; }
        public DateTime Lancamento { get; set; }
        public string? Genero { get; set; }
        public int DuracaoMinutos { get; set; }
    }
}
