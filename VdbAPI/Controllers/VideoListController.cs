using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VdbAPI.Models;
using static System.Net.WebRequestMethods;

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
        public async Task<ActionResult<IEnumerable<VideoListDTO>>> GetVideoLists()
        {
            var query = _context.VideoLists.AsQueryable();

            var videoListDTOs = await query
               .Select(v => new VideoListDTO
               {
                   VideoId = v.VideoId,
                   VideoName = v.VideoName,
                   TypeId = v.TypeId,
                   TypeName = v.Type.TypeName,
                   Summary = v.Summary,
                   SeriesId = v.SeriesId,
                   SeriesName = v.Series.SeriesName,
                   SeasonId = v.SeasonId,
                   SeasonName = v.Season.SeasonName,
                   MainGenreId = v.MainGenreId,
                   MainGenreName = v.MainGenre.GenreName,
                   ThumbnailPath = v.ThumbnailPath,
                   Bgpath = v.Bgpath

               }).ToListAsync();

            return Ok(videoListDTOs);
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
        public async Task<ActionResult<IEnumerable<VideoListDTO>>> SearchVideos(
            string? videoName,
            int? typeId,
            string? summary,
            [FromQuery(Name = "genreNames")] List<string>? genreNames,
            string? seriesName,
            string? seasonName)
        {
            var query = _context.VideoLists
                .Include(v => v.MainGenre)
                .Include(v => v.Series)
                .Include(v => v.Season)
                .Include(v => v.Type)
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

            if (genreNames != null && genreNames.Any())
            {
                query = query.Where(v => genreNames.Contains(v.MainGenre.GenreName));
            }

            if (!string.IsNullOrEmpty(seriesName))
            {
                query = query.Where(v => v.Series.SeriesName.Contains(seriesName));
            }

            if (!string.IsNullOrEmpty(seasonName))
            {
                query = query.Where(v => v.Season.SeasonName.Contains(seasonName));
            }

            var videoListDTOs = await query
                .Select(v => new VideoListDTO
                {
                    VideoId = v.VideoId,
                    VideoName = v.VideoName,
                    TypeId = v.TypeId,
                    TypeName = v.Type.TypeName,
                    Summary = v.Summary,
                    SeriesId = v.SeriesId,
                    SeriesName = v.Series.SeriesName,
                    SeasonId = v.SeasonId,
                    SeasonName = v.Season.SeasonName,
                    MainGenreId = v.MainGenreId,
                    MainGenreName = v.MainGenre.GenreName,
                    ThumbnailPath = v.ThumbnailPath,
                    Bgpath = v.Bgpath

                }).ToListAsync();

            return Ok(videoListDTOs);
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
        //[HttpPost]
        //public async Task<ActionResult<VideoList>> PostVideoList(VideoList videoList)
        //{
        //    _context.VideoLists.Add(videoList);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetVideoList", new { id = videoList.VideoId }, videoList);
        //}

        [HttpPost("newVideo={videoName}")]
        public async Task<IActionResult> PostVideoList([FromBody] VideoCreateDTO videoDTO)
        {

            TimeSpan parsedTime;
            if (!string.IsNullOrEmpty(videoDTO.RunningTime))
            {
                parsedTime = TimeSpan.Parse(videoDTO.RunningTime);
            }
            else
            {
                parsedTime= TimeSpan.Zero;
            }
            var video = new VideoList
            {
                VideoName = videoDTO.VideoName,
                TypeId = videoDTO.TypeId,
                SeriesId = videoDTO.SeriesId,
                MainGenreId = videoDTO.MainGenreId,
                RunningTime = parsedTime,
                IsShowing = videoDTO.IsShowing,
                ReleaseDate = videoDTO.ReleaseDate,
                Lang = videoDTO.Lang,
                Summary = videoDTO.Summary,
                AgeRating = videoDTO.AgeRating,
                TrailerUrl = videoDTO.TrailerUrl,
                ThumbnailPath = videoDTO.ThumbnailPath,
                Bgpath = videoDTO.Bgpath    

            };
            _context.VideoLists.Add(video);
            await _context.SaveChangesAsync();

            // Now handle images
            foreach (var imageDTO in videoDTO.Images)
            {
                var image = new ImageList { ImagePath = imageDTO.ImagePath };
                _context.ImageLists.Add(image);
                await _context.SaveChangesAsync();

                var imageForVideo = new ImageForVideoList
                {
                    VideoId = video.VideoId,
                    ImageId = image.ImageId
                };
                _context.ImageForVideoLists.Add(imageForVideo);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVideoList", new { id = video.VideoId }, video);
        }

        [HttpPost("uploadImages")]
        public async Task<IActionResult> UploadImages([FromForm] IFormFile thumbnail, [FromForm] List<IFormFile> images)
        {
            // 處理縮圖圖片
            string thumbnailPath = string.Empty;
            if (thumbnail != null)
            {
                var filePath = Path.Combine("wwwroot/assets/img", thumbnail.FileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await thumbnail.CopyToAsync(stream);
                }

                // 複製到 Angular 專案的 assets 目錄
                var angularAssetsPath = Path.Combine("../AngularFront/src/assets/img", thumbnail.FileName);
                System.IO.File.Copy(filePath, angularAssetsPath, overwrite: true);

                thumbnailPath = "/assets/img/" + thumbnail.FileName;
            }

            // 處理其他圖片
            List<string> imagePaths = new List<string>();
            foreach (var image in images)
            {
                var filePath = Path.Combine("wwwroot/assets/img", image.FileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await image.CopyToAsync(stream);
                }

                // 複製到 Angular 專案的 assets 目錄
                var angularAssetsPath = Path.Combine("../AngularFront/src/assets/img", image.FileName);
                System.IO.File.Copy(filePath, angularAssetsPath, overwrite: true);

                imagePaths.Add("/assets/img/" + image.FileName);
            }
            

            return Ok(new { thumbnailPath, imagePaths });
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
