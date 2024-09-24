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
    public class KeywordForVideoListController : ControllerBase
    {
        private readonly VideoDBContext _context;

        public KeywordForVideoListController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: api/KeywordForVideoList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KeywordForVideoList>>> GetKeywordForVideoLists()
        {
            return await _context.KeywordForVideoLists.ToListAsync();
        }

        // GET: api/KeywordForVideoList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<KeywordForVideoList>> GetKeywordForVideoList(int id)
        {
            var keywordForVideoList = await _context.KeywordForVideoLists.FindAsync(id);

            if (keywordForVideoList == null)
            {
                return NotFound();
            }

            return keywordForVideoList;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<string>>> SearchKeywords(string query)
        {
            var keywords = await _context.KeywordLists
                .Where(k => k.Keyword.Contains(query))
                .Select(k => k.Keyword)
                .ToListAsync();

            return Ok(keywords);
        }

        [HttpGet("GetKeywordsByVideo/{videoId}")]
        public async Task<ActionResult<IEnumerable<KeywordDTO>>> GetKeywordsByVideo(int videoId)
        {
            // 查詢所有與指定 VideoID 相關的關鍵字
            var keywords = await _context.KeywordForVideoLists
                                         .Where(k => k.VideoId == videoId)
                                         .Select(k => new KeywordDTO
                                         {
                                             KeywordId = k.KeywordId.Value, // 確保 KeywordId 不為 null
                                             Keyword = k.Keyword.Keyword
                                         })
                                         .ToListAsync();

            if (keywords == null || !keywords.Any())
            {
                return NotFound("No keywords found for the specified video.");
            }

            return Ok(keywords);
        }

        // PUT: api/KeywordForVideoList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKeywordForVideoList(int id, KeywordForVideoList keywordForVideoList)
        {
            if (id != keywordForVideoList.SerialId)
            {
                return BadRequest();
            }

            _context.Entry(keywordForVideoList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KeywordForVideoListExists(id))
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

        // POST: api/KeywordForVideoList
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<KeywordForVideoList>> PostKeywordForVideoList(KeywordForVideoList keywordForVideoList)
        {
            _context.KeywordForVideoLists.Add(keywordForVideoList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKeywordForVideoList", new { id = keywordForVideoList.SerialId }, keywordForVideoList);
        }

        [HttpPost("AddKeywordToVideo/{videoId}")]
        public async Task<IActionResult> AddKeywordToVideo(int videoId, string keyword)
        {

            if (string.IsNullOrEmpty(keyword))
            {
                return BadRequest("關鍵字不可為空");
            }
            // 先檢查關鍵字是否已經存在於總表中
            var existingKeyword = await _context.KeywordLists
                                                .FirstOrDefaultAsync(k => k.Keyword == keyword);

            // 如果關鍵字不存在，則新增到 KeywordList 總表
            if (existingKeyword == null)
            {
                existingKeyword = new KeywordList
                {
                    Keyword = keyword
                };
                _context.KeywordLists.Add(existingKeyword);
                await _context.SaveChangesAsync();
            }

            // 檢查該關鍵字是否已經與該影片有關聯
            var existingKeywordForVideo = await _context.KeywordForVideoLists
                                                        .FirstOrDefaultAsync(k => k.VideoId == videoId && k.KeywordId == existingKeyword.KeywordId);

            // 如果還沒有關聯，則新增到 KeywordForVideoList
            if (existingKeywordForVideo == null)
            {
                var keywordForVideo = new KeywordForVideoList
                {
                    VideoId = videoId,
                    KeywordId = existingKeyword.KeywordId
                };

                _context.KeywordForVideoLists.Add(keywordForVideo);
                await _context.SaveChangesAsync();
            }
            else
            {
                return Conflict("該關鍵字已經存在於影片中");
            }

            return Ok("關鍵字成功新增到影片");
        }

        [HttpDelete("remove/{videoId}/{keywordId}")]
        public async Task<ActionResult> RemoveKeywordFromVideo(int videoId, int keywordId)
        {
            var keywordForVideo = await _context.KeywordForVideoLists
                .FirstOrDefaultAsync(k => k.VideoId == videoId && k.KeywordId == keywordId);

            if (keywordForVideo != null)
            {
                _context.KeywordForVideoLists.Remove(keywordForVideo);
                await _context.SaveChangesAsync();

                // 檢查關鍵字是否還被其他影片使用
                var isUsedByOtherVideos = await _context.KeywordForVideoLists
                    .AnyAsync(k => k.KeywordId == keywordId);

                if (!isUsedByOtherVideos)
                {
                    // 如果沒有其他影片使用該關鍵字，從 KeywordList 中刪除
                    var keyword = await _context.KeywordLists.FindAsync(keywordId);
                    if (keyword != null)
                    {
                        _context.KeywordLists.Remove(keyword);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return Ok();
        }

        // DELETE: api/KeywordForVideoList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKeywordForVideoList(int id)
        {
            var keywordForVideoList = await _context.KeywordForVideoLists.FindAsync(id);
            if (keywordForVideoList == null)
            {
                return NotFound();
            }

            _context.KeywordForVideoLists.Remove(keywordForVideoList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KeywordForVideoListExists(int id)
        {
            return _context.KeywordForVideoLists.Any(e => e.SerialId == id);
        }
    }
}
