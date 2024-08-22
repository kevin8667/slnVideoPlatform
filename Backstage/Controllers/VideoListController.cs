using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backstage.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Dynamic.Core;


// TODO: 將部分欄位統一至Overlay
namespace Backstage.Controllers
{
    public class VideoListController : Controller
    {
        private readonly VideoDBContext _context;

        public VideoListController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: VideoList
        public async Task<IActionResult> Index()
        {
            var videoDBContext = _context.VideoLists.Include(v => v.MainGenre).Include(v => v.Season).Include(v => v.Series).Include(v => v.Type);

            var typeList = _context.TypeLists.Select(t => new { t.TypeId, t.TypeName }).ToList();

            ViewBag.TypeFilter = new SelectList(typeList, "TypeId", "TypeName");

            return View(await videoDBContext.ToListAsync());
        }

        

        // GET: VideoList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videoList = await _context.VideoLists
                .Include(v => v.MainGenre)
                .Include(v => v.Season)
                .Include(v => v.Series)
                .Include(v => v.Type)
                .FirstOrDefaultAsync(m => m.VideoId == id);
            if (videoList == null)
            {
                return NotFound();
            }

            return View(videoList);
        }

        public async Task<IActionResult> GetVideoDetails(int videoId)
        {
            var video = await _context.VideoLists
                .Where(v => v.VideoId == videoId)
                .Select(v => new {
                    Series = v.Series.SeriesName != null ? v.Series.SeriesName.ToString() : string.Empty,
                    Season = v.Season.SeasonNumber != null ? v.Season.SeasonNumber.ToString() : string.Empty,
                    Episode = v.Episode != null ? v.Episode.ToString() : string.Empty,
                    MainGenre = v.MainGenre.GenreName,
                    RunningTime = v.RunningTime != null ? v.RunningTime.ToString() : string.Empty,
                    IsShowing = v.IsShowing ? "上映中" : "已下檔",
                    ReleaseDate = v.ReleaseDate != null ? v.ReleaseDate.ToString() : string.Empty,
                    Lang = v.Lang != null ? v.Lang.ToString() : string.Empty,
                    AgeRating = v.AgeRating != null ? v.AgeRating.ToString() : string.Empty,
                    Summary = !string.IsNullOrEmpty(v.Summary) ? v.Summary : string.Empty,
                }).FirstOrDefaultAsync();

            if (video == null)
            {
                return NotFound();
            }

            return Json(video);
        }
        public async Task<IActionResult> GetImage(int videoId)
        {
            // 根據 videoId 查找 VideoList 中的 ThumbnailId
            var video = await _context.VideoLists
                .Where(v => v.VideoId == videoId)
                .Select(v => new { v.ThumbnailId })
                .FirstOrDefaultAsync();

            if (video == null || video.ThumbnailId == null)
            {
                return Json(new { success = false, message = "找不到對應的影片或縮圖。" });
            }

            // 根據 ThumbnailId 查找 ImageList 中的 ImagePath
            var image = await _context.ImageLists
                .Where(i => i.ImageId == video.ThumbnailId)
                .Select(i => new { i.ImagePath })
                .FirstOrDefaultAsync();

            if (image == null)
            {
                return Json(new { success = false, message = "找不到對應的圖片。" });
            }

            // 組合圖片的完整 URL (假設圖片儲存在 wwwroot/img 下)
            var imageUrl = Url.Content(image.ImagePath);

            return Json(new { success = true, imageUrl });
        }

        // GET: VideoList/Create
        public IActionResult Create()
        {
            ViewData["MainGenreId"] = new SelectList(_context.GenreLists, "GenreId", "GenreName");
            ViewData["SeasonId"] = new SelectList(_context.SeasonLists, "SeasonId", "SeasonName");
            ViewData["SeriesId"] = new SelectList(_context.SeriesLists, "SeriesId", "SeriesName");
            ViewData["TypeId"] = new SelectList(_context.TypeLists, "TypeId", "TypeName");
            ViewBag.SeasonList = new SelectList(_context.SeasonLists, "SeasonId", "SeasonName");
            return View();
        }

        // POST: VideoList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VideoId,TypeId,VideoName,SeriesId,MainGenreId,SeasonId,Episode,RunningTime,IsShowing,ReleaseDate,Rating,Popularity,ThumbnailId,Lang,Summary,Views,AgeRating,TrailerUrl")] VideoList videoList)
        {
            

            if (ModelState.IsValid)
            {
                _context.Add(videoList);
                await _context.SaveChangesAsync();

                var genresForVideo = new GenresForVideoList
                {
                    VideoId = videoList.VideoId,
                    GenreId = videoList.MainGenreId
                };
                _context.GenresForVideoLists.Add(genresForVideo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MainGenreId"] = new SelectList(_context.GenreLists, "GenreId", "GenreName", videoList.MainGenreId);
            ViewData["SeasonId"] = new SelectList(_context.SeasonLists, "SeasonId", "SeasonName", videoList.SeasonId);
            ViewData["SeriesId"] = new SelectList(_context.SeriesLists, "SeriesId", "SeriesName", videoList.SeriesId);
            ViewData["TypeId"] = new SelectList(_context.TypeLists, "TypeId", "TypeName", videoList.TypeId);
            ViewBag.SeasonList = new SelectList(_context.SeasonLists, "SeasonId", "SeasonName");
            return View(videoList);
        }

        // GET: VideoList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videoList = await _context.VideoLists.FindAsync(id);
            if (videoList == null)
            {
                return NotFound();
            }

            ViewBag.VideoId = videoList.VideoId;

            ViewData["MainGenreId"] = new SelectList(_context.GenreLists, "GenreId", "GenreName", videoList.MainGenreId);
            ViewData["SeasonId"] = new SelectList(_context.SeasonLists, "SeasonId", "SeasonName", videoList.SeasonId);
            ViewData["SeriesId"] = new SelectList(_context.SeriesLists, "SeriesId", "SeriesName", videoList.SeriesId);
            ViewData["TypeId"] = new SelectList(_context.TypeLists, "TypeId", "TypeName", videoList.TypeId);
            ViewBag.SeasonList = new SelectList(_context.SeasonLists, "SeasonId", "SeasonName");
            return View(videoList);
        }

        // POST: VideoList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VideoId,TypeId,VideoName,SeriesId,MainGenreId,SeasonId,Episode,RunningTime,IsShowing,ReleaseDate,Rating,Popularity,ThumbnailId,Lang,Summary,Views,AgeRating,TrailerUrl")] VideoList videoList)
        {
            if (id != videoList.VideoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(videoList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoListExists(videoList.VideoId))
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
            ViewData["MainGenreId"] = new SelectList(_context.GenreLists, "GenreId", "GenreName", videoList.MainGenreId);
            ViewData["SeasonId"] = new SelectList(_context.SeasonLists, "SeasonId", "SeasonName", videoList.SeasonId);
            ViewData["SeriesId"] = new SelectList(_context.SeriesLists, "SeriesId", "SeriesName", videoList.SeriesId);
            ViewData["TypeId"] = new SelectList(_context.TypeLists, "TypeId", "TypeName", videoList.TypeId);
            ViewBag.SeasonList = new SelectList(_context.SeasonLists, "SeasonId", "SeasonName");
            return View(videoList);
        }

        public async Task<IActionResult> EditSummary(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videoList = await _context.VideoLists.FindAsync(id);
            if (videoList == null)
            {
                return NotFound();
            }
            return View(videoList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSummary(int id, [Bind("VideoId,Summary")] VideoList videoList)
        {
            if (id != videoList.VideoId)
            {
                return NotFound();
            }

            var videoListFound = await _context.VideoLists.FindAsync(id);
            if (videoListFound == null)
            {
                return NotFound();
            }
            videoListFound.Summary = videoList.Summary;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(videoListFound);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoListExists(videoListFound.VideoId))
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
            return View(videoListFound.Summary);
        }

        

        // GET: VideoList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videoList = await _context.VideoLists
                .Include(v => v.MainGenre)
                .Include(v => v.Season)
                .Include(v => v.Series)
                .Include(v => v.Type)
                .FirstOrDefaultAsync(m => m.VideoId == id);
            if (videoList == null)
            {
                return NotFound();
            }
            ViewBag.VideoId = videoList.VideoId;

            return View(videoList);
        }

        // POST: VideoList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var videoList = await _context.VideoLists.FindAsync(id);
            if (videoList != null)
            {
                _context.VideoLists.Remove(videoList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoListExists(int id)
        {
            return _context.VideoLists.Any(e => e.VideoId == id);
        }
    }
}
