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
    public class KeywordListController : ControllerBase
    {
        private readonly VideoDBContext _context;

        public KeywordListController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: api/KeywordList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KeywordList>>> GetKeywordLists()
        {
            return await _context.KeywordLists.ToListAsync();
        }

        // GET: api/KeywordList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<KeywordList>> GetKeywordList(int id)
        {
            var keywordList = await _context.KeywordLists.FindAsync(id);

            if (keywordList == null)
            {
                return NotFound();
            }

            return keywordList;
        }

        // PUT: api/KeywordList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKeywordList(int id, KeywordList keywordList)
        {
            if (id != keywordList.KeywordId)
            {
                return BadRequest();
            }

            _context.Entry(keywordList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KeywordListExists(id))
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

        // POST: api/KeywordList
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<KeywordList>> PostKeywordList(KeywordList keywordList)
        {
            _context.KeywordLists.Add(keywordList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKeywordList", new { id = keywordList.KeywordId }, keywordList);
        }

        // DELETE: api/KeywordList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKeywordList(int id)
        {
            var keywordList = await _context.KeywordLists.FindAsync(id);
            if (keywordList == null)
            {
                return NotFound();
            }

            _context.KeywordLists.Remove(keywordList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KeywordListExists(int id)
        {
            return _context.KeywordLists.Any(e => e.KeywordId == id);
        }
    }
}
