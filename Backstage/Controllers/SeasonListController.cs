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
    public class SeasonListController : Controller
    {
        private readonly VideoDBContext _context;

        public SeasonListController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: SeasonList
        public async Task<IActionResult> Index()
        {
            var seasonLists = await _context.SeasonLists
            .Include(s => s.Series)  // 包含 SeriesList 的資料
            .ToListAsync();

            return View(seasonLists);
            //return View(await _context.SeasonLists.ToListAsync());
        }

        // GET: SeasonList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seasonList = await _context.SeasonLists
                .FirstOrDefaultAsync(m => m.SeasonId == id);
            if (seasonList == null)
            {
                return NotFound();
            }

            return View(seasonList);
        }

        // GET: SeasonList/Create
        //public IActionResult Create()
        //{
        //    // 獲取所有的 Series 資料
        //    ViewData["SeriesId"] = new SelectList(_context.SeriesLists, "SeriesId", "SeriesName");

        //    return View();
        //}
        public IActionResult Create(int? seriesId)
        {
            // 獲取所有的 Series 資料
            ViewData["SeriesId"] = new SelectList(_context.SeriesLists, "SeriesId", "SeriesName");

            // 如果有 seriesId，設定為預設值
            var model = new SeasonList
            {
                SeriesId = seriesId ?? 0 // 如果 seriesId 是 null，則設為 0
            };

            return View(model);
        }

        // POST: SeasonList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SeasonId,SeriesId,SeasonName,SeasonNumber,EpisodeCount,ReleaseDate,Summary")] SeasonList seasonList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(seasonList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(seasonList);
        }

        // GET: SeasonList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seasonList = await _context.SeasonLists.FindAsync(id);
            if (seasonList == null)
            {
                return NotFound();
            }
            return View(seasonList);
        }

        // POST: SeasonList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SeasonId,SeriesId,SeasonName,SeasonNumber,EpisodeCount,ReleaseDate,Summary")] SeasonList seasonList)
        {
            if (id != seasonList.SeasonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seasonList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeasonListExists(seasonList.SeasonId))
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
            return View(seasonList);
        }

        // GET: SeasonList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seasonList = await _context.SeasonLists
                .FirstOrDefaultAsync(m => m.SeasonId == id);
            if (seasonList == null)
            {
                return NotFound();
            }

            return View(seasonList);
        }

        // POST: SeasonList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seasonList = await _context.SeasonLists.FindAsync(id);
            if (seasonList != null)
            {
                _context.SeasonLists.Remove(seasonList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeasonListExists(int id)
        {
            return _context.SeasonLists.Any(e => e.SeasonId == id);
        }
    }
}
