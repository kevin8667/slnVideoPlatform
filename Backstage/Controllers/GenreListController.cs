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
    public class GenreListController : Controller
    {
        private readonly VideoDBContext _context;

        public GenreListController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: GenreList
        public async Task<IActionResult> Index()
        {
            return View(await _context.GenreLists.ToListAsync());
        }

        // GET: GenreList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genreList = await _context.GenreLists
                .FirstOrDefaultAsync(m => m.GenreId == id);
            if (genreList == null)
            {
                return NotFound();
            }

            return View(genreList);
        }

        // GET: GenreList/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GenreList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GenreId,GenreName")] GenreList genreList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genreList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(genreList);
        }

        // GET: GenreList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genreList = await _context.GenreLists.FindAsync(id);
            if (genreList == null)
            {
                return NotFound();
            }
            return View(genreList);
        }

        // POST: GenreList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GenreId,GenreName")] GenreList genreList)
        {
            if (id != genreList.GenreId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genreList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreListExists(genreList.GenreId))
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
            return View(genreList);
        }

        // GET: GenreList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genreList = await _context.GenreLists
                .FirstOrDefaultAsync(m => m.GenreId == id);
            if (genreList == null)
            {
                return NotFound();
            }

            return View(genreList);
        }

        // POST: GenreList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var genreList = await _context.GenreLists.FindAsync(id);
            if (genreList != null)
            {
                _context.GenreLists.Remove(genreList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GenreListExists(int id)
        {
            return _context.GenreLists.Any(e => e.GenreId == id);
        }
    }
}
