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
    public class KeywordForVideoListController : ControllerBase
    {
        private readonly VideoDBContext _context;

        public KeywordForVideoListController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: api/KeywordForVideoList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KeywordForVideoList>>> GetKeywordForVideoLists()
        {
            return await _context.KeywordForVideoLists.ToListAsync();
        }

        // GET: api/KeywordForVideoList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<KeywordForVideoList>> GetKeywordForVideoList(int id)
        {
            var keywordForVideoList = await _context.KeywordForVideoLists.FindAsync(id);

            if (keywordForVideoList == null)
            {
                return NotFound();
            }

            return keywordForVideoList;
        }

        // PUT: api/KeywordForVideoList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKeywordForVideoList(int id, KeywordForVideoList keywordForVideoList)
        {
            if (id != keywordForVideoList.SerialId)
            {
                return BadRequest();
            }

            _context.Entry(keywordForVideoList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KeywordForVideoListExists(id))
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

        // POST: api/KeywordForVideoList
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<KeywordForVideoList>> PostKeywordForVideoList(KeywordForVideoList keywordForVideoList)
        {
            _context.KeywordForVideoLists.Add(keywordForVideoList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKeywordForVideoList", new { id = keywordForVideoList.SerialId }, keywordForVideoList);
        }

        // DELETE: api/KeywordForVideoList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKeywordForVideoList(int id)
        {
            var keywordForVideoList = await _context.KeywordForVideoLists.FindAsync(id);
            if (keywordForVideoList == null)
            {
                return NotFound();
            }

            _context.KeywordForVideoLists.Remove(keywordForVideoList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KeywordForVideoListExists(int id)
        {
            return _context.KeywordForVideoLists.Any(e => e.SerialId == id);
        }
    }
}
