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
    public class DirectorListController : ControllerBase
    {
        private readonly VideoDBContext _context;

        public DirectorListController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: api/DirectorList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DirectorList>>> GetDirectorLists()
        {
            return await _context.DirectorLists.ToListAsync();
        }

        // GET: api/DirectorList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DirectorList>> GetDirectorList(int id)
        {
            var directorList = await _context.DirectorLists.FindAsync(id);

            if (directorList == null)
            {
                return NotFound();
            }

            return directorList;
        }

        // PUT: api/DirectorList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDirectorList(int id, DirectorList directorList)
        {
            if (id != directorList.DirectorId)
            {
                return BadRequest();
            }

            _context.Entry(directorList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DirectorListExists(id))
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

        // POST: api/DirectorList
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DirectorList>> PostDirectorList(DirectorList directorList)
        {
            _context.DirectorLists.Add(directorList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDirectorList", new { id = directorList.DirectorId }, directorList);
        }

        // DELETE: api/DirectorList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDirectorList(int id)
        {
            var directorList = await _context.DirectorLists.FindAsync(id);
            if (directorList == null)
            {
                return NotFound();
            }

            _context.DirectorLists.Remove(directorList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DirectorListExists(int id)
        {
            return _context.DirectorLists.Any(e => e.DirectorId == id);
        }
    }
}
