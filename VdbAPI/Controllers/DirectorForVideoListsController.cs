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
    public class DirectorForVideoListsController : ControllerBase
    {
        private readonly VideoDBContext _context;

        public DirectorForVideoListsController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: api/DirectorForVideoLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DirectorForVideoList>>> GetDirectorForVideoLists()
        {
            return await _context.DirectorForVideoLists.ToListAsync();
        }

        // GET: api/DirectorForVideoLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DirectorForVideoList>> GetDirectorForVideoList(int id)
        {
            var directorForVideoList = await _context.DirectorForVideoLists.FindAsync(id);

            if (directorForVideoList == null)
            {
                return NotFound();
            }

            return directorForVideoList;
        }

        [HttpGet("GetDirectorsByVideoId/{videoId}")]
        public async Task<ActionResult<IEnumerable<DirectorList>>> GetDirectorsByVideoId(int videoId)
        {
            // 查詢與指定影片相關的導演列表
            var directors = await _context.DirectorForVideoLists
                .Where(d => d.VideoId == videoId)
                .Select(d => d.Director) // 選擇完整的 DirectorList
                .ToListAsync();

            if (directors == null || directors.Count == 0)
            {
                return NotFound("No directors found for the specified video.");
            }

            return Ok(directors);
        }

        // PUT: api/DirectorForVideoLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDirectorForVideoList(int id, DirectorForVideoList directorForVideoList)
        {
            if (id != directorForVideoList.SerialId)
            {
                return BadRequest();
            }

            _context.Entry(directorForVideoList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DirectorForVideoListExists(id))
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

        // POST: api/DirectorForVideoLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DirectorForVideoList>> PostDirectorForVideoList(DirectorForVideoList directorForVideoList)
        {
            _context.DirectorForVideoLists.Add(directorForVideoList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDirectorForVideoList", new { id = directorForVideoList.SerialId }, directorForVideoList);
        }

        // DELETE: api/DirectorForVideoLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDirectorForVideoList(int id)
        {
            var directorForVideoList = await _context.DirectorForVideoLists.FindAsync(id);
            if (directorForVideoList == null)
            {
                return NotFound();
            }

            _context.DirectorForVideoLists.Remove(directorForVideoList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DirectorForVideoListExists(int id)
        {
            return _context.DirectorForVideoLists.Any(e => e.SerialId == id);
        }
    }
}
