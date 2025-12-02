using System.Net.Http;
using System.Threading.Tasks;
using CineReview.Api.Services;

namespace CineReview.Api.Services
{
    public interface IApiExternaService
    {
        Task<string> BuscarFilmeOMDb(string titulo);
    }

    public class ApiExternaService : IApiExternaService
    {
        private readonly HttpClient _httpClient;

        public ApiExternaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> BuscarFilmeOMDb(string titulo)
        {
            string apiKey = "SUA_CHAVE_OMDB";
            var response = await _httpClient.GetAsync($"https://www.omdbapi.com/?t={titulo}&apikey={apiKey}");
            return await response.Content.ReadAsStringAsync();
        }
    }
}
