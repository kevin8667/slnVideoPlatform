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
    public class GenresForVideoListsController : ControllerBase
    {
        private readonly VideoDBContext _context;

        public GenresForVideoListsController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: api/GenresForVideoLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenresForVideoList>>> GetGenresForVideoLists()
        {
            return await _context.GenresForVideoLists.ToListAsync();
        }

        // GET: api/GenresForVideoLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GenresForVideoList>> GetGenresForVideoList(int id)
        {
            var genresForVideoList = await _context.GenresForVideoLists.FindAsync(id);

            if (genresForVideoList == null)
            {
                return NotFound();
            }

            return genresForVideoList;
        }

        // PUT: api/GenresForVideoLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenresForVideoList(int id, GenresForVideoList genresForVideoList)
        {
            if (id != genresForVideoList.SerialId)
            {
                return BadRequest();
            }

            _context.Entry(genresForVideoList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenresForVideoListExists(id))
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

        // POST: api/GenresForVideoLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GenresForVideoList>> PostGenresForVideoList(GenresForVideoList genresForVideoList)
        {
            _context.GenresForVideoLists.Add(genresForVideoList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGenresForVideoList", new { id = genresForVideoList.SerialId }, genresForVideoList);
        }

        // DELETE: api/GenresForVideoLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenresForVideoList(int id)
        {
            var genresForVideoList = await _context.GenresForVideoLists.FindAsync(id);
            if (genresForVideoList == null)
            {
                return NotFound();
            }

            _context.GenresForVideoLists.Remove(genresForVideoList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GenresForVideoListExists(int id)
        {
            return _context.GenresForVideoLists.Any(e => e.SerialId == id);
        }
    }
}
