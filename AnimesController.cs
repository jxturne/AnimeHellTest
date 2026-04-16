using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AnimeHellTest.Models;
using AnimeHellTest.Services;
using JikanDotNet;
using Anime = AnimeHellTest.Models.Anime;

namespace AnimeHellTest
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimesController : ControllerBase
    {
        private readonly AnimeDB _context;
        private readonly AnimeService _animeService;
        private readonly IJikan _jikan;

        public AnimesController(AnimeDB context, AnimeService animeService, IJikan jikan)
        {
            _context = context;
            _animeService = animeService;
            _jikan = jikan;
        }

        // GET: api/Animes
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Animes.ToListAsync());
        }

        // GET: api/Animes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anime = await _context.Animes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (anime == null)
            {
                return NotFound();
            }

            return Ok(anime);
        }

        // POST: api/Animes
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Anime anime)
        {
            if (ModelState.IsValid)
            {
                _context.Add(anime);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Details), new { id = anime.ID }, anime);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/Animes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Anime anime)
        {
            if (id != anime.ID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(anime); 
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimeExists(anime.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok(anime);
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/Animes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var anime = await _context.Animes.FindAsync(id);
            if (anime == null)
            {
                return NotFound();
            }

            _context.Animes.Remove(anime);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        private bool AnimeExists(int id)
        {
            return _context.Animes.Any(e => e.ID == id);
        }
    }
}
