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
    public class PlanListsController : ControllerBase
    {
        private readonly VideoDBContext _context;

        public PlanListsController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: api/PlanLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlanList>>> GetPlanLists()
        {
            return await _context.PlanLists.ToListAsync();
        }

        // GET: api/PlanLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlanList>> GetPlanList(int id)
        {
            var planList = await _context.PlanLists.FindAsync(id);

            if (planList == null)
            {
                return NotFound();
            }

            return planList;
        }

        // PUT: api/PlanLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlanList(int id, PlanList planList)
        {
            if (id != planList.PlanId)
            {
                return BadRequest();
            }

            _context.Entry(planList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanListExists(id))
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

        // POST: api/PlanLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PlanList>> PostPlanList(PlanList planList)
        {
            _context.PlanLists.Add(planList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlanList", new { id = planList.PlanId }, planList);
        }

        // DELETE: api/PlanLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlanList(int id)
        {
            var planList = await _context.PlanLists.FindAsync(id);
            if (planList == null)
            {
                return NotFound();
            }

            _context.PlanLists.Remove(planList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlanListExists(int id)
        {
            return _context.PlanLists.Any(e => e.PlanId == id);
        }
    }
}
