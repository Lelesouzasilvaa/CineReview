using System.ComponentModel.DataAnnotations.Schema;

namespace CineReview.Api.Models
{
    [Table("Filmes")]
    public class Filme : Midia
    {
        public string Diretor { get; set; } = "";
        public int DuracaoMinutos { get; set; }
        public DateTime Lancamento { get; set; }
        public string Genero { get; set; } = "";
    }
}
