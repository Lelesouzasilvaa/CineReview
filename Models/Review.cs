namespace CineReview.Api.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int MidiaId { get; set; }       // FK genérica para Filme ou Serie
        public string Tipo { get; set; } = ""; // "Filme" ou "Serie"
        public double Nota { get; set; }
        public string? Comentario { get; set; }

        public Usuario? Usuario { get; set; }
        public Filme? Filme { get; set; }
        public Serie? Serie { get; set; }
        public Midia? Midia { get; set; }
    }
}

