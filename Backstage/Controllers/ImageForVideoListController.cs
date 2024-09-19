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

        //[HttpGet]
        //public async Task<IActionResult> EditThumbnail(int videoId)
        //{
        //    Console.WriteLine($"VideoId received: {videoId}");
        //    if (videoId == 0)
        //    {
        //        return BadRequest();
        //    }
        //    var imageForVideoList = await _context.ImageForVideoLists
        //        .Include(iv => iv.Image)
        //        .Include(iv => iv.Video)
        //        .Where(iv => iv.VideoId == videoId)
        //        .FirstOrDefaultAsync();

        //    if (imageForVideoList == null)
        //    {
        //        return NotFound();
        //    }

        //    // Create a SelectList for ImageList
        //    ViewBag.ImageList = new SelectList(
        //        _context.ImageLists,
        //        "ImageId",
        //        "ImagePath", // or another property you want to display
        //        imageForVideoList.ImageId
        //    );

        //    // Pass the VideoId to the view to use it in the form
        //    ViewBag.VideoId = videoId;

        //    return View(imageForVideoList);
        //}

        // POST: ImageForVideoList/EditThumbnail/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditThumbnail(int videoId, [Bind("ImageId, VideoId")] ImageForVideoList imageForVideoList)
        //{
        //    if (videoId != imageForVideoList.VideoId)
        //    {
        //        return NotFound();
        //    }

        //    var existingImageForVideoList = await _context.ImageForVideoLists
        //        .FirstOrDefaultAsync(iv => iv.VideoId == videoId);

        //    if (existingImageForVideoList == null)
        //    {
        //        return NotFound();
        //    }

        //    // 更新 ImageForVideoList 中的 ImageId
        //    existingImageForVideoList.ImageId = imageForVideoList.ImageId;

        //    // 獲取對應的 VideoList 並更新 ThumbnailId
        //    var video = await _context.VideoLists.FirstOrDefaultAsync(v => v.VideoId == videoId);
        //    if (video != null)
        //    {
        //        video.ThumbnailId = imageForVideoList.ImageId;
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            // 保存更改
        //            _context.Update(existingImageForVideoList);
        //            _context.Update(video);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ImageForVideoListExists(existingImageForVideoList.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }

        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ImageList = new SelectList(
        //        _context.ImageLists,
        //        "ImageId",
        //        "ImagePath",
        //        existingImageForVideoList.ImageId
        //    );

        //    ViewBag.VideoId = videoId;
        //    return View(existingImageForVideoList);
        //}


        private bool ImageForVideoListExists(int id)
        {
            return _context.ImageForVideoLists.Any(e => e.Id == id);
        }
    }
}
