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
    public class MemberRatingsController : ControllerBase
    {
        private readonly VideoDBContext _context;

        public MemberRatingsController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: api/MemberRatings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberRating>>> GetMemberRatings()
        {
            return await _context.MemberRatings.ToListAsync();
        }

        // GET: api/MemberRatings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberRating>> GetMemberRating(int id)
        {
            var memberRating = await _context.MemberRatings.FindAsync(id);

            if (memberRating == null)
            {
                return NotFound();
            }

            return memberRating;
        }



        // PUT: api/MemberRatings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMemberRating(int id, MemberRating memberRating)
        {
            if (id != memberRating.RatingId)
            {
                return BadRequest();
            }

            _context.Entry(memberRating).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberRatingExists(id))
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

        // POST: api/MemberRatings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MemberRating>> PostMemberRating(MemberRating memberRating)
        {
            _context.MemberRatings.Add(memberRating);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMemberRating", new { id = memberRating.RatingId }, memberRating);
        }

        // DELETE: api/MemberRatings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMemberRating(int id)
        {
            var memberRating = await _context.MemberRatings.FindAsync(id);
            if (memberRating == null)
            {
                return NotFound();
            }

            _context.MemberRatings.Remove(memberRating);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MemberRatingExists(int id)
        {
            return _context.MemberRatings.Any(e => e.RatingId == id);
        }
    }
}
