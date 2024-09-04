using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VdbAPI.Models;

namespace VdbAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreListController : ControllerBase
    {
        private readonly VideoDBContext _context;

        public GenreListController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: api/GenreList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreList>>> GetGenreLists()
        {
            return await _context.GenreLists.ToListAsync();
        }

        // GET: api/GenreList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GenreList>> GetGenreList(int id)
        {
            var genreList = await _context.GenreLists.FindAsync(id);

            if (genreList == null)
            {
                return NotFound();
            }

            return genreList;
        }

        // PUT: api/GenreList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenreList(int id, GenreList genreList)
        {
            if (id != genreList.GenreId)
            {
                return BadRequest();
            }

            _context.Entry(genreList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/GenreList
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GenreList>> PostGenreList(GenreList genreList)
        {
            _context.GenreLists.Add(genreList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGenreList", new { id = genreList.GenreId }, genreList);
        }

        // DELETE: api/GenreList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenreList(int id)
        {
            var genreList = await _context.GenreLists.FindAsync(id);
            if (genreList == null)
            {
                return NotFound();
            }

            _context.GenreLists.Remove(genreList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GenreListExists(int id)
        {
            return _context.GenreLists.Any(e => e.GenreId == id);
        }
    }
}
