using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VdbAPI.Models;

namespace VdbAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageForVideoListController : ControllerBase
    {
        private readonly VideoDBContext _context;

        public ImageForVideoListController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: api/ImageForVideoList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageForVideoList>>> GetImageForVideoLists()
        {
            return await _context.ImageForVideoLists.ToListAsync();
        }

        // GET: api/ImageForVideoList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageForVideoList>> GetImageForVideoList(int id)
        {
            var imageForVideoList = await _context.ImageForVideoLists.FindAsync(id);

            if (imageForVideoList == null)
            {
                return NotFound();
            }

            return imageForVideoList;
        }

        [HttpGet("video={videoId}")]
        public ActionResult<List<string>> GetImagePathsByVideoId(int videoId)
        {
            var imagePaths = _context.ImageForVideoLists
                .Where(iv => iv.VideoId == videoId)
                .Select(iv => iv.Image.ImagePath)
                .ToList();

            if (imagePaths == null || imagePaths.Count == 0)
            {
                return NotFound();
            }

            return Ok(imagePaths);
        }

        // PUT: api/ImageForVideoList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImageForVideoList(int id, ImageForVideoList imageForVideoList)
        {
            if (id != imageForVideoList.Id)
            {
                return BadRequest();
            }

            _context.Entry(imageForVideoList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageForVideoListExists(id))
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

        // POST: api/ImageForVideoList
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ImageForVideoList>> PostImageForVideoList(ImageForVideoList imageForVideoList)
        {
            _context.ImageForVideoLists.Add(imageForVideoList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImageForVideoList", new { id = imageForVideoList.Id }, imageForVideoList);
        }

        // DELETE: api/ImageForVideoList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImageForVideoList(int id)
        {
            var imageForVideoList = await _context.ImageForVideoLists.FindAsync(id);
            if (imageForVideoList == null)
            {
                return NotFound();
            }

            _context.ImageForVideoLists.Remove(imageForVideoList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImageForVideoListExists(int id)
        {
            return _context.ImageForVideoLists.Any(e => e.Id == id);
        }
    }
}
