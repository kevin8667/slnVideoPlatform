using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backstage.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        // GET: VideoList/Create
        public IActionResult Create()
        {
            ViewData["MainGenreId"] = new SelectList(_context.GenreLists, "GenreId", "GenreName");
            ViewData["SeasonId"] = new SelectList(_context.SeasonLists, "SeasonId", "SeasonId");
            ViewData["SeriesId"] = new SelectList(_context.SeriesLists, "SeriesId", "SeriesName");
            ViewData["TypeId"] = new SelectList(_context.TypeLists, "TypeId", "TypeName");
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
            ViewData["SeasonId"] = new SelectList(_context.SeasonLists, "SeasonId", "SeasonId", videoList.SeasonId);
            ViewData["SeriesId"] = new SelectList(_context.SeriesLists, "SeriesId", "SeriesName", videoList.SeriesId);
            ViewData["TypeId"] = new SelectList(_context.TypeLists, "TypeId", "TypeName", videoList.TypeId);
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
            ViewData["MainGenreId"] = new SelectList(_context.GenreLists, "GenreId", "GenreName", videoList.MainGenreId);
            ViewData["SeasonId"] = new SelectList(_context.SeasonLists, "SeasonId", "SeasonId", videoList.SeasonId);
            ViewData["SeriesId"] = new SelectList(_context.SeriesLists, "SeriesId", "SeriesName", videoList.SeriesId);
            ViewData["TypeId"] = new SelectList(_context.TypeLists, "TypeId", "TypeName", videoList.TypeId);
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
            ViewData["SeasonId"] = new SelectList(_context.SeasonLists, "SeasonId", "SeasonId", videoList.SeasonId);
            ViewData["SeriesId"] = new SelectList(_context.SeriesLists, "SeriesId", "SeriesName", videoList.SeriesId);
            ViewData["TypeId"] = new SelectList(_context.TypeLists, "TypeId", "TypeName", videoList.TypeId);
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

        public async Task<IActionResult> EditThumbnail(int? id)
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
        public async Task<IActionResult> EditThumbnail(int id, [Bind("VideoId,ThumbnailId")] VideoList videoList)
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
            videoListFound.ThumbnailId = videoList.ThumbnailId;

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
