using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VdbAPI.Models;

namespace VdbAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoListController : ControllerBase
    {
        private readonly VideoDBContext _context;

        public VideoListController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: api/VideoList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VideoList>>> GetVideoLists()
        {
            return await _context.VideoLists.ToListAsync();
        }

        // GET: api/VideoList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VideoList>> GetVideoList(int id)
        {
            var videoList = await _context.VideoLists.FindAsync(id);

            if (videoList == null)
            {
                return NotFound();
            }

            return videoList;
        }
        [HttpGet("type={typeID}")]
        public async Task<ActionResult<VideoList>> GetVideoListByType(int typeID)
        {
            var videoList = await _context.VideoLists
                .Where(v => v.TypeId == typeID)
                .ToListAsync();

            if (videoList == null)
            {
                return NotFound();
            }

            return Ok(videoList);
        }

        [HttpGet("search")]
        public async Task<ActionResult<PaginatedResponse<VideoListDTO>>> SearchVideos(
            string? videoName,
            int? typeId,
            string? summary,
            string? genreName,
            string? seriesName,
            string? seasonName,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = _context.VideoLists
                .Include(v => v.MainGenre)
                .Include(v => v.Series)
                .Include(v => v.Season)
                .Include(v => v.Type) // 如果需要 Type 名稱
                .AsQueryable();

            if (!string.IsNullOrEmpty(videoName))
            {
                query = query.Where(v => v.VideoName.Contains(videoName));
            }

            if (typeId.HasValue)
            {
                query = query.Where(v => v.TypeId == typeId);
            }

            if (!string.IsNullOrEmpty(summary))
            {
                query = query.Where(v => v.Summary.Contains(summary));
            }

            if (!string.IsNullOrEmpty(genreName))
            {
                query = query.Where(v => v.MainGenre.GenreName.Contains(genreName));
            }

            if (!string.IsNullOrEmpty(seriesName))
            {
                query = query.Where(v => v.Series.SeriesName.Contains(seriesName));
            }

            if (!string.IsNullOrEmpty(seasonName))
            {
                query = query.Where(v => v.Season.SeasonName.Contains(seasonName));
            }

            // 總記錄數量
            var totalRecords = await query.CountAsync();

            // 應用分頁
            var videoListDTOs = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(v => new VideoListDTO
                {
                    VideoId = v.VideoId,
                    VideoName = v.VideoName,
                    TypeId = v.TypeId,
                    TypeName = v.Type.TypeName, // 假設 Type 有 TypeName 屬性
                    Summary = v.Summary,
                    SeriesId = v.SeriesId,
                    SeriesName = v.Series.SeriesName,
                    SeasonId = v.SeasonId,
                    SeasonName = v.Season.SeasonName,
                    MainGenreId = v.MainGenreId,
                    MainGenreName = v.MainGenre.GenreName
                }).ToListAsync();

            // 返回分頁結果
            var response = new PaginatedResponse<VideoListDTO>
            {
                Items = videoListDTOs,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return Ok(response);
        }

        // PUT: api/VideoList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVideoList(int id, VideoList videoList)
        {
            if (id != videoList.VideoId)
            {
                return BadRequest();
            }

            _context.Entry(videoList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/VideoList
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VideoList>> PostVideoList(VideoList videoList)
        {
            _context.VideoLists.Add(videoList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVideoList", new { id = videoList.VideoId }, videoList);
        }

        // DELETE: api/VideoList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideoList(int id)
        {
            var videoList = await _context.VideoLists.FindAsync(id);
            if (videoList == null)
            {
                return NotFound();
            }

            _context.VideoLists.Remove(videoList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VideoListExists(int id)
        {
            return _context.VideoLists.Any(e => e.VideoId == id);
        }
    }
}
