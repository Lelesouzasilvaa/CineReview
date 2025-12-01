public interface IAuthServices
{
    Task<string?> Login(string email, string senha);
}
