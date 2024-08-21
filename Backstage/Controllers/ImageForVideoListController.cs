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
    public class ImageForVideoListController : Controller
    {
        private readonly VideoDBContext _context;

        public ImageForVideoListController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: ImageForVideoList
        public async Task<IActionResult> Index()
        {
            var videoDBContext = _context.ImageForVideoLists.Include(i => i.Image).Include(i => i.Video);
            return View(await videoDBContext.ToListAsync());
        }

        // GET: ImageForVideoList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imageForVideoList = await _context.ImageForVideoLists
                .Include(i => i.Image)
                .Include(i => i.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imageForVideoList == null)
            {
                return NotFound();
            }

            return View(imageForVideoList);
        }

        // GET: ImageForVideoList/Create
        public IActionResult Create()
        {
            var images = _context.ImageLists.Select(i => new SelectListItem
            {
                Value = i.ImageId.ToString(),
                Text = Path.GetFileName(i.ImagePath) // 顯示檔案名稱而不是 ID
            }).ToList();

            ViewBag.ImageId = images;
            //ViewData["ImageId"] = new SelectList(_context.ImageLists, "ImageId", "ImageId");
            ViewData["VideoId"] = new SelectList(_context.VideoLists, "VideoId", "VideoName");
            return View();
        }

        // POST: ImageForVideoList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImageId,VideoId")] ImageForVideoList imageForVideoList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(imageForVideoList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ImageId"] = new SelectList(_context.ImageLists, "ImageId", "ImageId", imageForVideoList.ImageId);
            ViewData["VideoId"] = new SelectList(_context.VideoLists, "VideoId", "VideoName", imageForVideoList.VideoId);
            return View(imageForVideoList);
        }

        // GET: ImageForVideoList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imageForVideoList = await _context.ImageForVideoLists.FindAsync(id);
            if (imageForVideoList == null)
            {
                return NotFound();
            }
            ViewData["ImageId"] = new SelectList(_context.ImageLists, "ImageId", "ImageId", imageForVideoList.ImageId);
            ViewData["VideoId"] = new SelectList(_context.VideoLists, "VideoId", "VideoName", imageForVideoList.VideoId);
            return View(imageForVideoList);
        }

        // POST: ImageForVideoList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ImageId,VideoId")] ImageForVideoList imageForVideoList)
        {
            if (id != imageForVideoList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imageForVideoList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageForVideoListExists(imageForVideoList.Id))
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
            ViewData["ImageId"] = new SelectList(_context.ImageLists, "ImageId", "ImageId", imageForVideoList.ImageId);
            ViewData["VideoId"] = new SelectList(_context.VideoLists, "VideoId", "VideoName", imageForVideoList.VideoId);
            return View(imageForVideoList);
        }

        // GET: ImageForVideoList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imageForVideoList = await _context.ImageForVideoLists
                .Include(i => i.Image)
                .Include(i => i.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imageForVideoList == null)
            {
                return NotFound();
            }

            return View(imageForVideoList);
        }

        // POST: ImageForVideoList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imageForVideoList = await _context.ImageForVideoLists.FindAsync(id);
            if (imageForVideoList != null)
            {
                _context.ImageForVideoLists.Remove(imageForVideoList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult GetImage(int videoId)
        {
            // 查找所有與 VideoID 關聯的 ImageID
            var imageIds = _context.ImageForVideoLists
                .Where(iv => iv.VideoId == videoId)
                .Select(iv => iv.ImageId)
                .ToList();

            if (imageIds.Count == 0)
            {
                return NotFound();
            }

            // 查找第一個 ImageID 對應的 ImagePath
            var imagePath = _context.ImageLists
                .Where(i => imageIds.Contains(i.ImageId))
                .Select(i => i.ImagePath)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(imagePath))
            {
                return NotFound();
            }

            var fullImagePath = Url.Content(imagePath);
            return Json(new { success = true, imageUrl = fullImagePath });
        }

        private bool ImageForVideoListExists(int id)
        {
            return _context.ImageForVideoLists.Any(e => e.Id == id);
        }
    }
}
