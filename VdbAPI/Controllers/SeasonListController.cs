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
    public class SeasonListController : ControllerBase
    {
        private readonly VideoDBContext _context;

        public SeasonListController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: api/SeasonList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeasonList>>> GetSeasonLists()
        {
            return await _context.SeasonLists.ToListAsync();
        }

        // GET: api/SeasonList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SeasonList>> GetSeasonList(int id)
        {
            var seasonList = await _context.SeasonLists.FindAsync(id);

            if (seasonList == null)
            {
                return NotFound();
            }

            return seasonList;
        }

        // PUT: api/SeasonList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeasonList(int id, SeasonList seasonList)
        {
            if (id != seasonList.SeasonId)
            {
                return BadRequest();
            }

            _context.Entry(seasonList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeasonListExists(id))
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

        // POST: api/SeasonList
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SeasonList>> PostSeasonList(SeasonList seasonList)
        {
            _context.SeasonLists.Add(seasonList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSeasonList", new { id = seasonList.SeasonId }, seasonList);
        }

        // DELETE: api/SeasonList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeasonList(int id)
        {
            var seasonList = await _context.SeasonLists.FindAsync(id);
            if (seasonList == null)
            {
                return NotFound();
            }

            _context.SeasonLists.Remove(seasonList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SeasonListExists(int id)
        {
            return _context.SeasonLists.Any(e => e.SeasonId == id);
        }
    }
}
