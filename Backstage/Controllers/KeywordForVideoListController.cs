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
    public class KeywordForVideoListController : Controller
    {
        private readonly VideoDBContext _context;

        public KeywordForVideoListController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: KeywordForVideoList
        public async Task<IActionResult> Index()
        {
            var videoDBContext = _context.KeywordForVideoLists.Include(k => k.Keyword).Include(k => k.Video);
            return View(await videoDBContext.ToListAsync());
        }

        // GET: KeywordForVideoList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var keywordForVideoList = await _context.KeywordForVideoLists
                .Include(k => k.Keyword)
                .Include(k => k.Video)
                .FirstOrDefaultAsync(m => m.SerialId == id);
            if (keywordForVideoList == null)
            {
                return NotFound();
            }

            return View(keywordForVideoList);
        }

        // GET: KeywordForVideoList/Create
        public IActionResult Create()
        {
            ViewData["KeywordId"] = new SelectList(_context.KeywordLists, "KeywordId", "KeywordId");
            ViewData["VideoId"] = new SelectList(_context.VideoLists, "VideoId", "VideoName");
            return View();
        }

        // POST: KeywordForVideoList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SerialId,KeywordId,VideoId")] KeywordForVideoList keywordForVideoList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(keywordForVideoList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KeywordId"] = new SelectList(_context.KeywordLists, "KeywordId", "KeywordId", keywordForVideoList.KeywordId);
            ViewData["VideoId"] = new SelectList(_context.VideoLists, "VideoId", "VideoName", keywordForVideoList.VideoId);
            return View(keywordForVideoList);
        }

        // GET: KeywordForVideoList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var keywordForVideoList = await _context.KeywordForVideoLists.FindAsync(id);
            if (keywordForVideoList == null)
            {
                return NotFound();
            }
            ViewData["KeywordId"] = new SelectList(_context.KeywordLists, "KeywordId", "KeywordId", keywordForVideoList.KeywordId);
            ViewData["VideoId"] = new SelectList(_context.VideoLists, "VideoId", "VideoName", keywordForVideoList.VideoId);
            return View(keywordForVideoList);
        }

        // POST: KeywordForVideoList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SerialId,KeywordId,VideoId")] KeywordForVideoList keywordForVideoList)
        {
            if (id != keywordForVideoList.SerialId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(keywordForVideoList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KeywordForVideoListExists(keywordForVideoList.SerialId))
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
            ViewData["KeywordId"] = new SelectList(_context.KeywordLists, "KeywordId", "KeywordId", keywordForVideoList.KeywordId);
            ViewData["VideoId"] = new SelectList(_context.VideoLists, "VideoId", "VideoName", keywordForVideoList.VideoId);
            return View(keywordForVideoList);
        }

        // GET: KeywordForVideoList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var keywordForVideoList = await _context.KeywordForVideoLists
                .Include(k => k.Keyword)
                .Include(k => k.Video)
                .FirstOrDefaultAsync(m => m.SerialId == id);
            if (keywordForVideoList == null)
            {
                return NotFound();
            }

            return View(keywordForVideoList);
        }

        // POST: KeywordForVideoList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var keywordForVideoList = await _context.KeywordForVideoLists.FindAsync(id);
            if (keywordForVideoList != null)
            {
                _context.KeywordForVideoLists.Remove(keywordForVideoList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KeywordForVideoListExists(int id)
        {
            return _context.KeywordForVideoLists.Any(e => e.SerialId == id);
        }
    }
}
