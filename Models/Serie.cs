using System.ComponentModel.DataAnnotations.Schema;

namespace CineReview.Api.Models
{
    [Table("Series")]
    public class Serie : Midia
    {
        public int Temporadas { get; set; }
        public int Episodios { get; set; }

    }
}
