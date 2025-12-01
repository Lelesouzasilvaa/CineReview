namespace CineReview.Api.DTOs.Auth
{
    public class UserReadDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string Email { get; set; } = "";
        public string Token { get; set; } = "";
    }
}
