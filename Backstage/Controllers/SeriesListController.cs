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
    public class SeriesListController : Controller
    {
        private readonly VideoDBContext _context;

        public SeriesListController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: SeriesList
        public async Task<IActionResult> Index()
        {
            return View(await _context.SeriesLists.ToListAsync());
        }

        // GET: SeriesList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seriesList = await _context.SeriesLists
                .FirstOrDefaultAsync(m => m.SeriesId == id);
            if (seriesList == null)
            {
                return NotFound();
            }

            return View(seriesList);
        }

        // GET: SeriesList/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SeriesList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SeriesId,SeriesName")] SeriesList seriesList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(seriesList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(seriesList);
        }

        // GET: SeriesList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seriesList = await _context.SeriesLists.FindAsync(id);
            if (seriesList == null)
            {
                return NotFound();
            }
            return View(seriesList);
        }

        // POST: SeriesList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SeriesId,SeriesName")] SeriesList seriesList)
        {
            if (id != seriesList.SeriesId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seriesList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeriesListExists(seriesList.SeriesId))
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
            return View(seriesList);
        }

        // GET: SeriesList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seriesList = await _context.SeriesLists
                .FirstOrDefaultAsync(m => m.SeriesId == id);
            if (seriesList == null)
            {
                return NotFound();
            }

            return View(seriesList);
        }

        // POST: SeriesList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seriesList = await _context.SeriesLists.FindAsync(id);
            if (seriesList != null)
            {
                _context.SeriesLists.Remove(seriesList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeriesListExists(int id)
        {
            return _context.SeriesLists.Any(e => e.SeriesId == id);
        }
    }
}
