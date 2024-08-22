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
    public class KeywordListController : Controller
    {
        private readonly VideoDBContext _context;

        public KeywordListController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: KeywordList
        public async Task<IActionResult> Index()
        {
            return View(await _context.KeywordLists.ToListAsync());
        }

        // GET: KeywordList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var keywordList = await _context.KeywordLists
                .FirstOrDefaultAsync(m => m.KeywordId == id);
            if (keywordList == null)
            {
                return NotFound();
            }

            return View(keywordList);
        }

        // GET: KeywordList/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KeywordList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KeywordId,Keyword")] KeywordList keywordList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(keywordList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(keywordList);
        }

        // GET: KeywordList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var keywordList = await _context.KeywordLists.FindAsync(id);
            if (keywordList == null)
            {
                return NotFound();
            }
            return View(keywordList);
        }

        // POST: KeywordList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KeywordId,Keyword")] KeywordList keywordList)
        {
            if (id != keywordList.KeywordId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(keywordList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KeywordListExists(keywordList.KeywordId))
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
            return View(keywordList);
        }

        // GET: KeywordList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var keywordList = await _context.KeywordLists
                .FirstOrDefaultAsync(m => m.KeywordId == id);
            if (keywordList == null)
            {
                return NotFound();
            }

            return View(keywordList);
        }

        // POST: KeywordList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var keywordList = await _context.KeywordLists.FindAsync(id);
            if (keywordList != null)
            {
                _context.KeywordLists.Remove(keywordList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KeywordListExists(int id)
        {
            return _context.KeywordLists.Any(e => e.KeywordId == id);
        }
    }
}
