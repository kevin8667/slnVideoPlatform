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
    public class ImageListController : Controller
    {
        private readonly VideoDBContext _context;

        public ImageListController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: ImageList
        public async Task<IActionResult> Index()
        {
            return View(await _context.ImageLists.ToListAsync());
        }

        // GET: ImageList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imageList = await _context.ImageLists
                .FirstOrDefaultAsync(m => m.ImageId == id);
            if (imageList == null)
            {
                return NotFound();
            }

            return View(imageList);
        }

        // GET: ImageList/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ImageList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImageId,ImagePath")] ImageList imageList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(imageList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(imageList);
        }

        // GET: ImageList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imageList = await _context.ImageLists.FindAsync(id);
            if (imageList == null)
            {
                return NotFound();
            }
            return View(imageList);
        }

        // POST: ImageList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ImageId,ImagePath")] ImageList imageList)
        {
            if (id != imageList.ImageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imageList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageListExists(imageList.ImageId))
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
            return View(imageList);
        }

        // GET: ImageList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imageList = await _context.ImageLists
                .FirstOrDefaultAsync(m => m.ImageId == id);
            if (imageList == null)
            {
                return NotFound();
            }

            return View(imageList);
        }

        // POST: ImageList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imageList = await _context.ImageLists.FindAsync(id);
            if (imageList != null)
            {
                _context.ImageLists.Remove(imageList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageListExists(int id)
        {
            return _context.ImageLists.Any(e => e.ImageId == id);
        }
    }
}
