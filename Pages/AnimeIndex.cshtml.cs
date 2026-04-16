using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AnimeHellTest.Models;
using AnimeHellTest.Services;
using Microsoft.OpenApi;

namespace AnimeHellTest.Pages
{
    public class AnimeIndexModel : PageModel
    {
        private readonly AnimeService _animeService;
        private readonly AnimeDB _context;

        public AnimeIndexModel(AnimeService animeService, AnimeDB context)
        {
            _animeService = animeService;
            _context = context;
        }

        public List<Anime> MyAnimes { get; set; } = new();
        public List<Anime>? SearchResults { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string? SearchQuery { get; set; }

        public async Task OnGetAsync()
        {
            // Load my saved animes
            MyAnimes = await _context.Animes.ToListAsync();

            // If there's a search query, search Jikan
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                SearchResults = await _animeService.SearchAnimesFromJikan(SearchQuery, 12);
            }
        }

        public async Task<IActionResult> OnPostImportAsync(long malId)
        {
            var anime = await _animeService.SaveAnimeToDatabase(malId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var anime = await _context.Animes.FindAsync(id);
            if (anime != null)
            {
                _context.Animes.Remove(anime);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}