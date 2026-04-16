using AnimeHellTest.Models;
using JikanDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Anime = AnimeHellTest.Models.Anime;


namespace AnimeHellTest.Services
{
    public class AnimeService
    {
        private readonly IJikan _jikan;
        private readonly AnimeDB _context;
        //private readonly ILogger<AnimeService> _logger;

        public AnimeService(AnimeDB context)
        {
            _jikan = new Jikan();
            _context = context;
        }

        public async Task<Models.Anime?> GetAnimeFromJikan(long malId)
        {
            try
            {
                var jikanAnime = await _jikan.GetAnimeAsync(malId);
                
                if (jikanAnime?.Data == null)
                {
                    return null;
                }

                var anime = new Anime
                {
                    MalID = (long)jikanAnime.Data.MalId,  // Add this
                    Title = jikanAnime.Data.Titles?.FirstOrDefault()?.Title ?? "Unknown",  // Fix obsolete warning
                    Description = jikanAnime.Data.Synopsis ?? "No description available",
                    StudioName = jikanAnime.Data.Studios?.FirstOrDefault()?.Name ?? "Unknown Studio",
                    Episodes = jikanAnime.Data.Episodes ?? 0,
                    Seasons = jikanAnime.Data.Season.HasValue ? 1 : 0,
                    IsCompleted = jikanAnime.Data.Status == "Finished Airing"
                };

                return anime;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<Anime>> SearchAnimesFromJikan(string query, int maxResults = 10) //limit exposure
        {
            try
            {
                var searchResult = await _jikan.SearchAnimeAsync(query);
                
                if (searchResult?.Data == null)
                {
                    return new List<Anime>();
                }

                var animes = searchResult.Data.Take(maxResults).Select( a => new Anime
                {
                    MalID = (long)a.MalId,
                    Title = a.Titles?.FirstOrDefault()?.Title ?? "Unknown",  // Fix obsolete warning
                    Description = a.Synopsis ?? "No description available",
                    StudioName = a.Studios?.FirstOrDefault()?.Name ?? "Unknown Studio",
                    Episodes = a.Episodes ?? 0,
                    Seasons = a.Season.HasValue ? 1 : 0,
                    IsCompleted = a.Status == "Finished Airing",
                    ImageUrl = a.Images?.JPG?.ImageUrl ?? a.Images?.JPG?.LargeImageUrl,// Add this
                }).ToList();

                return animes;
            }
            catch (Exception)
            {
                return new List<Anime>();
            }
        }

        public async Task<Anime?> SaveAnimeToDatabase(long malId)
        {

            var existingAnime = await _context.Animes.FirstOrDefaultAsync(a => a.MalID == malId);
            if (existingAnime != null)
            {
                return existingAnime;
            }
    
            var anime = await GetAnimeFromJikan(malId);
            
            if (anime != null)
            {
                _context.Animes.Add(anime);
                await _context.SaveChangesAsync();

                return anime;
            }

            return anime;
        }
    }
}

