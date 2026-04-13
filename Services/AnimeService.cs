using AnimeHellTest.Models;


namespace AnimeHellTest.Services
{
    public class AnimeService
    {
        HttpClient httpClient;
        AnimeDB _context;

        string JikanBaseURL = "https://api.jikan.moe/v4/anime";
        public AnimeService(HttpClient httpClient, AnimeDB context)
        {
            this.httpClient = httpClient;
            this._context = context;
        }

        public async Task<List<Anime>> GetAnimes()
        {
            var response = await httpClient.GetAsync("https://localhost:/api/animes");//
            if (response.IsSuccessStatusCode)
            {
                var animes = await response.Content.ReadFromJsonAsync<List<Anime>>();
                return animes;
            }
            return new List<Anime>();
        }
    }
}
