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
    public class DirectorListController : Controller
    {
        private readonly VideoDBContext _context;

        public DirectorListController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: DirectorList
        public async Task<IActionResult> Index()
        {
            return View(await _context.DirectorLists.ToListAsync());
        }

        // GET: DirectorList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var directorList = await _context.DirectorLists
                .FirstOrDefaultAsync(m => m.DirectorId == id);
            if (directorList == null)
            {
                return NotFound();
            }

            return View(directorList);
        }

        // GET: DirectorList/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DirectorList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DirectorId,DirectorName,DirectorImage")] DirectorList directorList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(directorList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(directorList);
        }

        // GET: DirectorList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var directorList = await _context.DirectorLists.FindAsync(id);
            if (directorList == null)
            {
                return NotFound();
            }
            return View(directorList);
        }

        // POST: DirectorList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DirectorId,DirectorName,DirectorImage")] DirectorList directorList)
        {
            if (id != directorList.DirectorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(directorList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DirectorListExists(directorList.DirectorId))
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
            return View(directorList);
        }

        // GET: DirectorList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var directorList = await _context.DirectorLists
                .FirstOrDefaultAsync(m => m.DirectorId == id);
            if (directorList == null)
            {
                return NotFound();
            }

            return View(directorList);
        }

        // POST: DirectorList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var directorList = await _context.DirectorLists.FindAsync(id);
            if (directorList != null)
            {
                _context.DirectorLists.Remove(directorList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DirectorListExists(int id)
        {
            return _context.DirectorLists.Any(e => e.DirectorId == id);
        }
    }
}
