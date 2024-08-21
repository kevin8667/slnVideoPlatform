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
    public class GenresForVideoListsController : Controller
    {
        private readonly VideoDBContext _context;

        public GenresForVideoListsController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: GenresForVideoLists
        public async Task<IActionResult> Index()
        {
            var videoDBContext = _context.GenresForVideoLists.Include(g => g.Genre).Include(g => g.Video);
            return View(await videoDBContext.ToListAsync());
        }

        // GET: GenresForVideoLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genresForVideoList = await _context.GenresForVideoLists
                .Include(g => g.Genre)
                .Include(g => g.Video)
                .FirstOrDefaultAsync(m => m.SerialId == id);
            if (genresForVideoList == null)
            {
                return NotFound();
            }

            return View(genresForVideoList);
        }

        // GET: GenresForVideoLists/Create
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.GenreLists, "GenreId", "GenreName");
            ViewData["VideoId"] = new SelectList(_context.VideoLists, "VideoId", "VideoName");
            return View();
        }

        // POST: GenresForVideoLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SerialId,VideoId,GenreId")] GenresForVideoList genresForVideoList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genresForVideoList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_context.GenreLists, "GenreId", "GenreName", genresForVideoList.GenreId);
            ViewData["VideoId"] = new SelectList(_context.VideoLists, "VideoId", "VideoName", genresForVideoList.VideoId);
            return View(genresForVideoList);
        }

        // GET: GenresForVideoLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genresForVideoList = await _context.GenresForVideoLists.FindAsync(id);
            if (genresForVideoList == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.GenreLists, "GenreId", "GenreName", genresForVideoList.GenreId);
            ViewData["VideoId"] = new SelectList(_context.VideoLists, "VideoId", "VideoName", genresForVideoList.VideoId);
            return View(genresForVideoList);
        }

        // POST: GenresForVideoLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SerialId,VideoId,GenreId")] GenresForVideoList genresForVideoList)
        {
            if (id != genresForVideoList.SerialId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genresForVideoList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenresForVideoListExists(genresForVideoList.SerialId))
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
            ViewData["GenreId"] = new SelectList(_context.GenreLists, "GenreId", "GenreName", genresForVideoList.GenreId);
            ViewData["VideoId"] = new SelectList(_context.VideoLists, "VideoId", "VideoName", genresForVideoList.VideoId);
            return View(genresForVideoList);
        }

        // GET: GenresForVideoLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genresForVideoList = await _context.GenresForVideoLists
                .Include(g => g.Genre)
                .Include(g => g.Video)
                .FirstOrDefaultAsync(m => m.SerialId == id);
            if (genresForVideoList == null)
            {
                return NotFound();
            }

            return View(genresForVideoList);
        }

        // POST: GenresForVideoLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var genresForVideoList = await _context.GenresForVideoLists.FindAsync(id);
            if (genresForVideoList != null)
            {
                _context.GenresForVideoLists.Remove(genresForVideoList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GenresForVideoListExists(int id)
        {
            return _context.GenresForVideoLists.Any(e => e.SerialId == id);
        }
    }
}
