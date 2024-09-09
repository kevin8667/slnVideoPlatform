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
    public class CastListController : ControllerBase
    {
        private readonly VideoDBContext _context;

        public CastListController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: api/CastList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CastList>>> GetCastLists()
        {
            return await _context.CastLists.ToListAsync();
        }

        // GET: api/CastList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CastList>> GetCastList(int id)
        {
            var castList = await _context.CastLists.FindAsync(id);

            if (castList == null)
            {
                return NotFound();
            }

            return castList;
        }

        // GET: api/CastList/actor/5
        [HttpGet("actor/{actorId}")]
        public async Task<ActionResult<IEnumerable<VideoList>>> GetVideosByActor(int actorId)
        {
            // 找到該演員的 CastList 內所有相關的 VideoID
            var videoIds = await _context.CastLists
                                        .Where(c => c.ActorId == actorId)
                                        .Select(c => c.VideoId)
                                        .ToListAsync();

            if (videoIds == null || !videoIds.Any())
            {
                return NotFound("No videos found for this actor.");
            }

            // 使用這些 VideoID 來取得對應的 VideoList
            var videos = await _context.VideoLists
                                        .Where(v => videoIds.Contains(v.VideoId))
                                        .ToListAsync();

            if (videos == null || !videos.Any())
            {
                return NotFound("No videos found.");
            }

            return Ok(videos);
        }

        // PUT: api/CastList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCastList(int id, CastList castList)
        {
            if (id != castList.CastId)
            {
                return BadRequest();
            }

            _context.Entry(castList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CastListExists(id))
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

        // POST: api/CastList
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CastList>> PostCastList(CastList castList)
        {
            _context.CastLists.Add(castList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCastList", new { id = castList.CastId }, castList);
        }

        // DELETE: api/CastList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCastList(int id)
        {
            var castList = await _context.CastLists.FindAsync(id);
            if (castList == null)
            {
                return NotFound();
            }

            _context.CastLists.Remove(castList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CastListExists(int id)
        {
            return _context.CastLists.Any(e => e.CastId == id);
        }
    }
}
