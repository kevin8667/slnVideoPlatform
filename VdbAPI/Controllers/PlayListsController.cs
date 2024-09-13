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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaylistDTO>>> GetPlayLists()
        {
            var playlists = await _context.PlayLists
                .Include(pl => pl.PlayListItems)
                    .ThenInclude(pli => pli.Video)
                .Select(pl => new PlaylistDTO
                {
                    PlayListId = pl.PlayListId,
                    PlayListName = pl.PlayListName,
                    PlayListDescription = pl.PlayListDescription,
                    ViewCount = pl.ViewCount,
                    LikeCount = pl.LikeCount,
                    AddedCount = pl.AddedCount,
                    SharedCount = pl.SharedCount,
                    ShowImage = pl.ShowImage,
                    
                    Videos = pl.PlayListItems.Select(pli => new PlaylistitemDTO
                    {
                        PlayListId = pli.PlayListId,
                        VideoId = pli.Video.VideoId,
                        VideoPosition = pli.VideoPosition,
                        VideoName = pli.Video.VideoName,
                        ThumbnailPath = pli.Video.ThumbnailPath,
                        Episode = pli.Video.Episode
                    }).ToList()
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
        public async Task<ActionResult<PlayList>> PostPlayList([FromBody] PlayListCreateDTO dto)
        {
            var playList = dto.PlayList;
            var videoIds = dto.VideoIds;
            var collaboratorIds = dto.CollaboratorIds;

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

            int position = 1;
            foreach (var videoId in videoIds.Distinct())
            {
                var playListItem = new PlayListItem
                {
                    PlayListId = playList.PlayListId,
                    VideoId = videoId,
                    VideoPosition = position++,
                    VideoAddedAt = DateTime.UtcNow
                };
                _context.PlayListItems.Add(playListItem);
            }

            foreach (var collaboratorId in collaboratorIds.Distinct().Where(id => id != 5))
            {
                var collaborator = new PlayListCollaborator
                {
                    PlayListId = playList.PlayListId,
                    MemberId = collaboratorId,
                    CollaboratorJoinedAt = DateTime.UtcNow,
                    CollaboratorActionType = "Added",
                    ActionTimestamp = DateTime.UtcNow
                };
                _context.PlayListCollaborators.Add(collaborator);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlayList), new { id = playList.PlayListId }, playList);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayList(int id, [FromBody] PlayListCreateDTO dto)
        {
            var playList = dto.PlayList;
            var videoIds = dto.VideoIds ?? new List<int>();
            var collaboratorIds = dto.CollaboratorIds ?? new List<int>();

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
            else            {
                
                existingPlayList.ShowImage = existingPlayList.ShowImage;
            }

            existingPlayList.PlayListUpdatedAt = DateTime.UtcNow;
            _context.Entry(existingPlayList).State = EntityState.Modified;
           
            var existingPlayListItems = await _context.PlayListItems.Where(item => item.PlayListId == id).ToListAsync();
            _context.PlayListItems.RemoveRange(existingPlayListItems);

            int position = 1;
            foreach (var videoId in videoIds.Distinct())
            {
                var playListItem = new PlayListItem
                {
                    PlayListId = id,
                    VideoId = videoId,
                    VideoPosition = position++,
                    VideoAddedAt = DateTime.UtcNow
                };
                _context.PlayListItems.Add(playListItem);
            }
            
            var existingCollaborators = await _context.PlayListCollaborators.Where(collab => collab.PlayListId == id).ToListAsync();
            _context.PlayListCollaborators.RemoveRange(existingCollaborators);

            foreach (var collaboratorId in collaboratorIds.Distinct().Where(id => id != 5))
            {
                var collaborator = new PlayListCollaborator
                {
                    PlayListId = id,
                    MemberId = collaboratorId,
                    CollaboratorJoinedAt = DateTime.UtcNow,
                    CollaboratorActionType = "Updated",
                    ActionTimestamp = DateTime.UtcNow
                };
                _context.PlayListCollaborators.Add(collaborator);
            }

            await _context.SaveChangesAsync();

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

        // GET: api/PlayList/videos
        [HttpGet("videos")]
        public async Task<ActionResult<IEnumerable<VideoListDTO>>> GetAllVideos()
        {
            var videos = await _context.VideoLists
                .Select(v => new VideoListDTO
                {
                    VideoId = v.VideoId,
                    VideoName = v.VideoName,
                    Episode = v.Episode,
                    ThumbnailPath = v.ThumbnailPath ?? "/assets/img/movie.png"
                })
                .ToListAsync();

            if (videos == null || !videos.Any())
            {
                return NotFound("No videos found.");
            }

            return Ok(videos);
        }

        [HttpGet("{id}/items")]
        public async Task<ActionResult<IEnumerable<PlaylistitemDTO>>> GetPlayListItems(int id)
        {
            try
            {
                var playListItems = await _context.PlayListItems
                    .Where(p => p.PlayListId == id)
                    .Select(p => new PlaylistitemDTO
                    {
                        PlayListId = p.PlayListId,
                        VideoId = p.VideoId,
                        VideoPosition = p.VideoPosition,
                        VideoName = _context.VideoLists
                            .Where(v => v.VideoId == p.VideoId)
                            .Select(v => v.VideoName)
                            .FirstOrDefault() ?? "Unknown",
                        
                        ThumbnailPath = _context.VideoLists
                            .Where(v => v.VideoId == p.VideoId)
                            .Select(v => v.ThumbnailPath)
                            .FirstOrDefault() ?? "/assets/img/movie.png",

                        Episode = _context.VideoLists
                            .Where(v => v.VideoId == p.VideoId)
                            .Select(v => v.Episode)
                            .FirstOrDefault()
                    }).ToListAsync();

                if (playListItems == null || !playListItems.Any())
                {
                    return NotFound();
                }

                return Ok(playListItems);
            }
            catch (Exception ex)
            {                
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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

        [HttpGet("collaborators/{playlistId?}")]
        public async Task<ActionResult<IEnumerable<MemberInfoDTO>>> GetCollaborators(int? playlistId)
        {
            if (playlistId.HasValue)
            {                
                var collaborators = await _context.PlayListCollaborators
                    .Where(c => c.PlayListId == playlistId.Value)
                    .Select(c => new MemberInfoDTO
                    {
                        MemberId = c.Member.MemberId,
                        MemberName = c.Member.MemberName,
                        PhotoPath = c.Member.PhotoPath ?? "/assets/img/memberooo.png"
                    })
                    .ToListAsync();

                return Ok(collaborators);
            }
            else
            {                
                var allCollaborators = await _context.MemberInfos
                    .Select(m => new MemberInfoDTO
                    {
                        MemberId = m.MemberId,
                        MemberName = m.MemberName,
                        PhotoPath = m.PhotoPath ?? "/assets/img/memberooo.png"
                    })
                    .ToListAsync();

                return Ok(allCollaborators);
            }
        }
    }
}
