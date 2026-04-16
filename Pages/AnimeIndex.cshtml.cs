using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AnimeHellTest.Models;
using AnimeHellTest.Services;
using Microsoft.OpenApi;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;

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

        [BindProperty]
        public List<Anime> MyAnimes { get; set; } = new List<Anime>();
        public List<Anime>? SearchResults { get; set; } = new List<Anime>();

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

        [TempData]
        public string? StatusMessage { get; set; }

        public async Task<IActionResult> OnPostImportAsync(long malId)
        {


            var anime = await _animeService.SaveAnimeToDatabase(malId);

            if (anime != null)
            {
                StatusMessage = $"✅ '{anime.Title}' added to your collection!";
            }
            else
            {
                StatusMessage = "❌ Failed to add anime to collection.";
            }

            return RedirectToPage(new { SearchQuery = SearchQuery });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var anime = await _context.Animes.FindAsync(id);
            if (anime != null)
            {
                _context.Animes.Remove(anime);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage(new { SearchQuery = SearchQuery });
        }


        public async Task<IActionResult> OnPostUpdateAsync(int id, long malId)
        {
            var existingAnime = await _context.Animes.FindAsync(id);
            if (existingAnime != null)
            {

                var updatedData = await _animeService.GetAnimeFromJikan(malId);

                if (updatedData != null)
                {

                    existingAnime.Title = updatedData.Title;
                    existingAnime.Description = updatedData.Description;
                    existingAnime.StudioName = updatedData.StudioName;
                    existingAnime.Episodes = updatedData.Episodes;
                    existingAnime.Seasons = updatedData.Seasons;
                    existingAnime.IsCompleted = updatedData.IsCompleted;
                    existingAnime.ImageUrl = updatedData.ImageUrl;

                    _context.Animes.Update(existingAnime);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage(new { SearchQuery = SearchQuery });
        }

        public async Task<IActionResult> OnPostClearAsync()
        {
            var allAnimes = await _context.Animes.ToListAsync();
            _context.Animes.RemoveRange(allAnimes);
            await _context.SaveChangesAsync();
            return RedirectToPage(new { SearchQuery = SearchQuery });

        }
        [BindProperty(SupportsGet = true)]
        public bool ShowCollection { get; set; } = true;

        public async Task<IActionResult> OnPostToggleCollectionAsync()
        {
            ShowCollection = !ShowCollection;
            StatusMessage = ShowCollection ? "📚 Collection displayed" : "📚 Collection hidden";
            return RedirectToPage(new { SearchQuery = SearchQuery, ShowCollection = ShowCollection });
        }

        //public async Task<IActionResult> OnGetCollectionDBContext()
        //    {

        //    }
    }
}
