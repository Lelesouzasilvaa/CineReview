namespace CineReview.Api.Models
{
    public class Avaliacao
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int MidiaId { get; set; }
        public double Nota { get; set; }
        public string? Comentario { get; set; }
    }
}