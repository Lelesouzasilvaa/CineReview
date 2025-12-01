namespace CineReview.Api.DTOs
{
    public class AvaliacaoCreateDto
    {
        public int UsuarioId { get; set; }
        public int MidiaId { get; set; }
        public double Nota { get; set; }
        public string? Comentario { get; set; }
    }
}