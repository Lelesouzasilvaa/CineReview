namespace CineReview.Api.DTOs.Reviews
{
    public class ReviewCreateDto
    {
        public int UsuarioId { get; set; }
        public int MidiaId { get; set; }
        public string Tipo { get; set; } = ""; // "Filme" ou "Serie"
        public double Nota { get; set; }
        public string? Comentario { get; set; }
    }

    public class ReviewResponseDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNome { get; set; } = "";
        public int MidiaId { get; set; }
        public string MidiaTitulo { get; set; } = "";
        public string Tipo { get; set; } = "";
        public double Nota { get; set; }
        public string? Comentario { get; set; }
    }
}
