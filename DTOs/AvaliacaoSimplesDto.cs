namespace CineReview.Api.DTOs
{
    public class AvaliacaoSimplesDto
    {
        public int UsuarioId { get; set; }
        public string UsuarioNome { get; set; } = "";

        public int MidiaId { get; set; }
        public string MidiaTitulo { get; set; } = "";
        public string TipoMidia { get; set; } = ""; // Filme ou Serie

        public double Nota { get; set; }
        public string? Comentario { get; set; }
    }
}