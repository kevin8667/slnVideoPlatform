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
    public class ViedoPlanListsController : Controller
    {
        private readonly VideoDBContext _context;

        public ViedoPlanListsController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: ViedoPlanLists
        public async Task<IActionResult> Index()
        {
            var videoDBContext = _context.ViedoPlanLists.Include(v => v.Plan).Include(v => v.Viedo);
            return View(await videoDBContext.ToListAsync());
        }

        // GET: ViedoPlanLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viedoPlanList = await _context.ViedoPlanLists
                .Include(v => v.Plan)
                .Include(v => v.Viedo)
                .FirstOrDefaultAsync(m => m.ViedoPlanId == id);
            if (viedoPlanList == null)
            {
                return NotFound();
            }

            return View(viedoPlanList);
        }

        // GET: ViedoPlanLists/Create
        public IActionResult Create()
        {
            ViewData["PlanId"] = new SelectList(_context.PlanLists, "PlanId", "PlanName");
            ViewData["ViedoId"] = new SelectList(_context.VideoLists, "VideoId", "VideoName");
            return View();
        }

        // POST: ViedoPlanLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ViedoPlanId,PlanId,ViedoId")] ViedoPlanList viedoPlanList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(viedoPlanList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlanId"] = new SelectList(_context.PlanLists, "PlanId", "PlanName", viedoPlanList.PlanId);
            ViewData["ViedoId"] = new SelectList(_context.VideoLists, "VideoId", "VideoName", viedoPlanList.ViedoId);
            return View(viedoPlanList);
        }

        // GET: ViedoPlanLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viedoPlanList = await _context.ViedoPlanLists.FindAsync(id);
            if (viedoPlanList == null)
            {
                return NotFound();
            }
            ViewData["PlanId"] = new SelectList(_context.PlanLists, "PlanId", "PlanName", viedoPlanList.PlanId);
            ViewData["ViedoId"] = new SelectList(_context.VideoLists, "VideoId", "VideoName", viedoPlanList.ViedoId);
            return View(viedoPlanList);
        }

        // POST: ViedoPlanLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ViedoPlanId,PlanId,ViedoId")] ViedoPlanList viedoPlanList)
        {
            if (id != viedoPlanList.ViedoPlanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viedoPlanList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ViedoPlanListExists(viedoPlanList.ViedoPlanId))
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
            ViewData["PlanId"] = new SelectList(_context.PlanLists, "PlanId", "PlanName", viedoPlanList.PlanId);
            ViewData["ViedoId"] = new SelectList(_context.VideoLists, "VideoId", "VideoName", viedoPlanList.ViedoId);
            return View(viedoPlanList);
        }

        // GET: ViedoPlanLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viedoPlanList = await _context.ViedoPlanLists
                .Include(v => v.Plan)
                .Include(v => v.Viedo)
                .FirstOrDefaultAsync(m => m.ViedoPlanId == id);
            if (viedoPlanList == null)
            {
                return NotFound();
            }

            return View(viedoPlanList);
        }

        // POST: ViedoPlanLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var viedoPlanList = await _context.ViedoPlanLists.FindAsync(id);
            if (viedoPlanList != null)
            {
                _context.ViedoPlanLists.Remove(viedoPlanList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ViedoPlanListExists(int id)
        {
            return _context.ViedoPlanLists.Any(e => e.ViedoPlanId == id);
        }
    }
}
