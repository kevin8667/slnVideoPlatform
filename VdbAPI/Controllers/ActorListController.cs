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
    public class ActorListController : ControllerBase
    {
        private readonly VideoDBContext _context;

        public ActorListController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: api/ActorList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorList>>> GetActorLists()
        {
            return await _context.ActorLists.ToListAsync();
        }

        // GET: api/ActorList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActorList>> GetActorList(int id)
        {
            var actorList = await _context.ActorLists.FindAsync(id);

            if (actorList == null)
            {
                return NotFound();
            }

            return actorList;
        }

        // PUT: api/ActorList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActorList(int id, ActorList actorList)
        {
            if (id != actorList.ActorId)
            {
                return BadRequest();
            }

            _context.Entry(actorList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActorListExists(id))
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

        // POST: api/ActorList
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ActorList>> PostActorList(ActorList actorList)
        {
            _context.ActorLists.Add(actorList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActorList", new { id = actorList.ActorId }, actorList);
        }

        // DELETE: api/ActorList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActorList(int id)
        {
            var actorList = await _context.ActorLists.FindAsync(id);
            if (actorList == null)
            {
                return NotFound();
            }

            _context.ActorLists.Remove(actorList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActorListExists(int id)
        {
            return _context.ActorLists.Any(e => e.ActorId == id);
        }
    }
}
