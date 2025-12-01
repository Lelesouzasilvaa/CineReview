namespace CineReview.Api.DTOs.Reviews
{
    public class ReviewReadDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }

        public string Tipo { get; set; } = string.Empty;
        public int MidiaId { get; set; }

        public double Nota { get; set; }
        public string? Comentario { get; set; }
    }
}
