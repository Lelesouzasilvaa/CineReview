namespace CineReview.Api.DTOs
{
    public class SerieCreateDto
    {
        public string Titulo { get; set; } = "";
        public string? Descricao { get; set; }
        public int Temporadas { get; set; }
        public int Episodios { get; set; }
    }
}
