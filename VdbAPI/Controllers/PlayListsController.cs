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
    public class PlayListController : ControllerBase
    {
        private readonly VideoDBContext _context;

        public PlayListController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: api/PlayList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaylistDTO>>> GetPlayLists()
        {
            var playlists = await _context.PlayLists
                .Select(pl => new PlaylistDTO
                {
                    PlayListName = pl.PlayListName,
                    PlayListDescription = pl.PlayListDescription,
                    ViewCount = pl.ViewCount,
                    LikeCount = pl.LikeCount,
                    AddedCount = pl.AddedCount,
                    SharedCount = pl.SharedCount,
                    ShowImage = pl.ShowImage
                })
                .ToListAsync();

            return Ok(playlists);
        }

        // GET: api/PlayList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlaylistDTO>> GetPlayList(int id)
        {
            var playlist = await _context.PlayLists
                .Where(pl => pl.PlayListId == id)
                .Select(pl => new PlaylistDTO
                {
                    PlayListName = pl.PlayListName,
                    PlayListDescription = pl.PlayListDescription,
                    ViewCount = pl.ViewCount,
                    LikeCount = pl.LikeCount,
                    AddedCount = pl.AddedCount,
                    SharedCount = pl.SharedCount,
                    ShowImage = pl.ShowImage
                })
                .FirstOrDefaultAsync();

            if (playlist == null)
            {
                return NotFound();
            }

            return Ok(playlist);
        }

        // POST: api/PlayList
        [HttpPost]
        public async Task<ActionResult<PlaylistDTO>> PostPlayList(PlaylistDTO playlistDto)
        {
            var playList = new PlayList
            {
                PlayListName = playlistDto.PlayListName,
                PlayListDescription = playlistDto.PlayListDescription,
                ViewCount = playlistDto.ViewCount,
                LikeCount = playlistDto.LikeCount,
                AddedCount = playlistDto.AddedCount,
                SharedCount = playlistDto.SharedCount,
                ShowImage = playlistDto.ShowImage,
                PlayListCreatedAt = DateTime.UtcNow,
                PlayListUpdatedAt = DateTime.UtcNow,
                AnalysisTimestamp = DateTime.UtcNow
            };

            _context.PlayLists.Add(playList);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlayList), new { id = playList.PlayListId }, playlistDto);
        }

        // PUT: api/PlayList/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayList(int id, PlaylistDTO playlistDto)
        {
            var playList = await _context.PlayLists.FindAsync(id);

            if (playList == null)
            {
                return NotFound();
            }

            playList.PlayListName = playlistDto.PlayListName;
            playList.PlayListDescription = playlistDto.PlayListDescription;
            playList.ViewCount = playlistDto.ViewCount;
            playList.LikeCount = playlistDto.LikeCount;
            playList.AddedCount = playlistDto.AddedCount;
            playList.SharedCount = playlistDto.SharedCount;
            playList.ShowImage = playlistDto.ShowImage;
            playList.PlayListUpdatedAt = DateTime.UtcNow;

            _context.Entry(playList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayListExists(id))
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

        // DELETE: api/PlayList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayList(int id)
        {
            var playList = await _context.PlayLists.FindAsync(id);
            if (playList == null)
            {
                return NotFound();
            }

            _context.PlayLists.Remove(playList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlayListExists(int id)
        {
            return _context.PlayLists.Any(e => e.PlayListId == id);
        }
    }
}
