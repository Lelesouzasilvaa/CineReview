namespace CineReview.Api.DTOs
{
    public class SerieReadDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = "";
        public string? Descricao { get; set; }
        public int Temporadas { get; set; }
        public int Episodios { get; set; }
        public double NotaMedia { get; set; }
    }
}
