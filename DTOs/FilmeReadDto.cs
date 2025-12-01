namespace CineReview.Api.DTOs
{
    public class FilmeReadDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Diretor { get; set; } = string.Empty;
        public DateTime Lancamento { get; set; }
        public string Genero { get; set; } = string.Empty;
        public int DuracaoMinutos { get; set; }
        public double NotaMedia { get; set; }
    }
}
