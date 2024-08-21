using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backstage.Models;

namespace Backstage.Controllers
{
    public class ActorListController : Controller
    {
        private readonly VideoDBContext _context;

        public ActorListController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: ActorList
        public async Task<IActionResult> Index()
        {
            return View(await _context.ActorLists.ToListAsync());
        }

        // GET: ActorList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actorList = await _context.ActorLists
                .FirstOrDefaultAsync(m => m.ActorId == id);
            if (actorList == null)
            {
                return NotFound();
            }

            return View(actorList);
        }

        // GET: ActorList/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActorList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActorId,ActorName,ActorImage")] ActorList actorList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actorList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actorList);
        }

        // GET: ActorList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actorList = await _context.ActorLists.FindAsync(id);
            if (actorList == null)
            {
                return NotFound();
            }
            return View(actorList);
        }

        // POST: ActorList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActorId,ActorName,ActorImage")] ActorList actorList)
        {
            if (id != actorList.ActorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actorList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorListExists(actorList.ActorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(actorList);
        }

        // GET: ActorList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actorList = await _context.ActorLists
                .FirstOrDefaultAsync(m => m.ActorId == id);
            if (actorList == null)
            {
                return NotFound();
            }

            return View(actorList);
        }

        // POST: ActorList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actorList = await _context.ActorLists.FindAsync(id);
            if (actorList != null)
            {
                _context.ActorLists.Remove(actorList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActorListExists(int id)
        {
            return _context.ActorLists.Any(e => e.ActorId == id);
        }
    }
}
