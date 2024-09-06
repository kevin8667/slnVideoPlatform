﻿using System;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaylistDTO>>> GetPlayLists()
        {
            var playlists = await _context.PlayLists
                .Select(pl => new PlaylistDTO
                {
                    PlayListId = pl.PlayListId,
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

        [HttpGet("{id}")]
        public async Task<ActionResult<PlaylistDTO>> GetPlayList(int id)
        {
            var playlist = await _context.PlayLists
                .Where(pl => pl.PlayListId == id)
                .Select(pl => new PlaylistDTO
                {
                    PlayListId = pl.PlayListId,
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

        [HttpPost]
        public async Task<ActionResult<PlayList>> PostPlayList(PlayList playList)
        {
            if (string.IsNullOrEmpty(playList.PlayListName))
            {
                return BadRequest("播放清單名稱為必填欄位");
            }

            if (string.IsNullOrEmpty(playList.PlayListDescription))
            {
                return BadRequest("播放清單描述為必填欄位");
            }
            
            if (!string.IsNullOrEmpty(playList.PlayListImage))
            {
                try
                {
                    playList.ShowImage = Convert.FromBase64String(playList.PlayListImage);
                }
                catch (FormatException)
                {
                    return BadRequest("圖片格式無效");
                }
            }
            
            playList.ViewCount = 100;
            playList.LikeCount = 100;
            playList.AddedCount = 100;
            playList.SharedCount = 100;
            
            playList.PlayListCreatedAt = DateTime.UtcNow;
            playList.PlayListUpdatedAt = DateTime.UtcNow;
            playList.AnalysisTimestamp = DateTime.UtcNow;
            
            _context.PlayLists.Add(playList);
            await _context.SaveChangesAsync();
            
            var memberCreatedPlayList = new MemberCreatedPlayList
            {
                MemberId = 5,
                PlayListId = playList.PlayListId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            _context.MemberCreatedPlayLists.Add(memberCreatedPlayList);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlayList), new { id = playList.PlayListId }, playList);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayList(int id, PlayList playList)
        {            
            if (id != playList.PlayListId)
            {
                return BadRequest("播放清單 ID 不匹配");
            }
            
            var existingPlayList = await _context.PlayLists.FindAsync(id);
            if (existingPlayList == null)
            {
                return NotFound("播放清單不存在");
            }
            
            existingPlayList.PlayListName = playList.PlayListName ?? existingPlayList.PlayListName;
            existingPlayList.PlayListDescription = playList.PlayListDescription ?? existingPlayList.PlayListDescription;
            existingPlayList.ViewCount = playList.ViewCount;
            existingPlayList.LikeCount = playList.LikeCount;
            existingPlayList.AddedCount = playList.AddedCount;
            existingPlayList.SharedCount = playList.SharedCount;
            
            if (!string.IsNullOrEmpty(playList.PlayListImage))
            {
                try
                {
                    existingPlayList.ShowImage = Convert.FromBase64String(playList.PlayListImage);
                }
                catch (FormatException)
                {
                    return BadRequest("圖片格式無效");
                }
            }
            
            existingPlayList.PlayListUpdatedAt = DateTime.UtcNow;
            
            _context.Entry(existingPlayList).State = EntityState.Modified;
            
            await _context.SaveChangesAsync();
           
            var memberCreatedPlayList = await _context.MemberCreatedPlayLists
                .Where(mcp => mcp.PlayListId == id && mcp.MemberId == 5)
                .FirstOrDefaultAsync();

            if (memberCreatedPlayList != null)
            {
                memberCreatedPlayList.UpdatedAt = DateTime.UtcNow;
                _context.Entry(memberCreatedPlayList).State = EntityState.Modified;
                await _context.SaveChangesAsync();
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

        [HttpGet("{id}/items")]
        public async Task<ActionResult<IEnumerable<PlaylistitemDTO>>> GetPlayListItems(int id)
        {
            var playListItems = await _context.PlayListItems
                .Where(p => p.PlayListId == id)
                .Select(p => new PlaylistitemDTO
                {
                    PlayListId = p.PlayListId,
                    VideoId = p.VideoId,
                    VideoPosition = p.VideoPosition,
                    VideoName = _context.VideoLists.FirstOrDefault(v => v.VideoId == p.VideoId).VideoName,
                    ThumbnailId = _context.VideoLists.FirstOrDefault(v => v.VideoId == p.VideoId).ThumbnailId,
                    Episode = _context.VideoLists.FirstOrDefault(v => v.VideoId == p.VideoId).Episode
                }).ToListAsync();

            if (playListItems == null || !playListItems.Any())
            {
                return NotFound();
            }

            return Ok(playListItems);
        }

        [HttpDelete("{id}/items/{videoId}")]
        public async Task<IActionResult> RemoveVideoFromPlayList(int id, int videoId)
        {
            var playListItem = await _context.PlayListItems
                .FirstOrDefaultAsync(p => p.PlayListId == id && p.VideoId == videoId);

            if (playListItem == null)
            {
                return NotFound();
            }

            _context.PlayListItems.Remove(playListItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}/items/{videoId}/position")]
        public async Task<IActionResult> UpdateVideoPosition(int id, int videoId, [FromBody] int newPosition)
        {
            var playListItem = await _context.PlayListItems
                .FirstOrDefaultAsync(p => p.PlayListId == id && p.VideoId == videoId);

            if (playListItem == null)
            {
                return NotFound();
            }

            playListItem.VideoPosition = newPosition;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayListItemExists(id, videoId))
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

        private bool PlayListItemExists(int playListId, int videoId)
        {
            return _context.PlayListItems.Any(p => p.PlayListId == playListId && p.VideoId == videoId);
        }

        [HttpGet("created/{memberId}")]
        public async Task<ActionResult<IEnumerable<PlaylistDTO>>> GetMemberCreatedPlaylists(int memberId)
        {
            var playlists = await _context.MemberCreatedPlayLists
                .Where(mcp => mcp.MemberId == memberId)
                .Select(mcp => new PlaylistDTO
                {
                    PlayListId = mcp.PlayListId,
                    PlayListName = _context.PlayLists.FirstOrDefault(pl => pl.PlayListId == mcp.PlayListId).PlayListName,
                    PlayListDescription = _context.PlayLists.FirstOrDefault(pl => pl.PlayListId == mcp.PlayListId).PlayListDescription,
                    ViewCount = _context.PlayLists.FirstOrDefault(pl => pl.PlayListId == mcp.PlayListId).ViewCount,
                    LikeCount = _context.PlayLists.FirstOrDefault(pl => pl.PlayListId == mcp.PlayListId).LikeCount,
                    AddedCount = _context.PlayLists.FirstOrDefault(pl => pl.PlayListId == mcp.PlayListId).AddedCount,
                    SharedCount = _context.PlayLists.FirstOrDefault(pl => pl.PlayListId == mcp.PlayListId).SharedCount,
                    ShowImage = _context.PlayLists.FirstOrDefault(pl => pl.PlayListId == mcp.PlayListId).ShowImage
                })
                .ToListAsync();

            return Ok(playlists);
        }

        [HttpGet("added/{memberId}")]
        public async Task<ActionResult<IEnumerable<PlaylistDTO>>> GetMemberAddedPlaylists(int memberId)
        {
            var playlists = await _context.MemberPlayLists
                .Where(mp => mp.MemberId == memberId)
                .Select(mp => new PlaylistDTO
                {
                    PlayListId = mp.PlayListId,
                    PlayListName = _context.PlayLists.FirstOrDefault(pl => pl.PlayListId == mp.PlayListId).PlayListName,
                    PlayListDescription = _context.PlayLists.FirstOrDefault(pl => pl.PlayListId == mp.PlayListId).PlayListDescription,
                    ViewCount = _context.PlayLists.FirstOrDefault(pl => pl.PlayListId == mp.PlayListId).ViewCount,
                    LikeCount = _context.PlayLists.FirstOrDefault(pl => pl.PlayListId == mp.PlayListId).LikeCount,
                    AddedCount = _context.PlayLists.FirstOrDefault(pl => pl.PlayListId == mp.PlayListId).AddedCount,
                    SharedCount = _context.PlayLists.FirstOrDefault(pl => pl.PlayListId == mp.PlayListId).SharedCount,
                    ShowImage = _context.PlayLists.FirstOrDefault(pl => pl.PlayListId == mp.PlayListId).ShowImage
                })
                .ToListAsync();

            return Ok(playlists);
        }

        [HttpGet("collaborator/{memberId}")]
        public async Task<ActionResult<IEnumerable<PlaylistDTO>>> GetMemberCollaboratorPlaylists(int memberId)
        {
            var playlists = await _context.PlayListCollaborators
                .Where(pc => pc.MemberId == memberId)
                .Select(pc => new PlaylistDTO
                {
                    PlayListId = pc.PlayListId,
                    PlayListName = _context.PlayLists.FirstOrDefault(pl => pl.PlayListId == pc.PlayListId).PlayListName,
                    PlayListDescription = _context.PlayLists.FirstOrDefault(pl => pl.PlayListId == pc.PlayListId).PlayListDescription,
                    ViewCount = _context.PlayLists.FirstOrDefault(pl => pl.PlayListId == pc.PlayListId).ViewCount,
                    LikeCount = _context.PlayLists.FirstOrDefault(pl => pl.PlayListId == pc.PlayListId).LikeCount,
                    AddedCount = _context.PlayLists.FirstOrDefault(pl => pl.PlayListId == pc.PlayListId).AddedCount,
                    SharedCount = _context.PlayLists.FirstOrDefault(pl => pl.PlayListId == pc.PlayListId).SharedCount,
                    ShowImage = _context.PlayLists.FirstOrDefault(pl => pl.PlayListId == pc.PlayListId).ShowImage
                })
                .ToListAsync();

            return Ok(playlists);
        }
    }
}
