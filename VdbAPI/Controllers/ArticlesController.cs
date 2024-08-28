using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using VdbAPI.DTO;
using VdbAPI.Models;

namespace VdbAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly VideoDBContext _context;

        public ArticlesController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: api/Articles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            return await _context.Articles.ToListAsync();

        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        // PUT: api/Articles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(int id, Article article)
        {
            if (id != article.ArticleId)
            {
                return BadRequest();
            }

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
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

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost()]
        public async Task<ActionResult<forumDto>> LoadIndex( forumDto searchDTO)
        {
            try {
                var article = _context.ArticleViews.AsQueryable();

                // 篩選條件
                if(searchDTO.categoryId != 0) {
                    article = article.Where(c => c.ThemeId == searchDTO.categoryId);
                }

                // 關鍵字篩選
                if(!string.IsNullOrEmpty(searchDTO.keyword)) {
                    article = article.Where(c => c.Title.Contains(searchDTO.keyword) ||
                                                 c.ArticleContent.Contains(searchDTO.keyword) ||
                                                 c.MemberName.Contains(searchDTO.keyword));
                }

                // 排序
                article = sortArticle(searchDTO,article);

                // 計算總筆數
                int dataCount = await article.CountAsync();

                // 分頁
                int pageSize = searchDTO.pageSize ?? 10;
                int page = searchDTO.page ?? 1;
                int totalPages = (int)Math.Ceiling((decimal)dataCount / pageSize);

                // 跳過指定頁數的資料並取出當前頁面的資料
                var articles = await article.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                // 準備回傳的 DTO
                var pagingDTO = new ForumPagingDTO {
                    TotalCount = dataCount,
                    TotalPages = totalPages,
                    ForumResult = articles
                };

                return Ok(pagingDTO); // 返回 OK 和 DTO
            }
            catch(Exception ex) {
                // 返回 500 錯誤和錯誤信息
                return StatusCode(StatusCodes.Status500InternalServerError,"錯誤原因:" + ex.Message);
            }
        }

        private static IQueryable<ArticleView> sortArticle(forumDto searchDTO,IQueryable<ArticleView> article)
        {
            switch(searchDTO.sortBy) {
                case "theme":
                article = searchDTO.sortType == "asc" ? article.OrderBy(s => s.ThemeId) :
                                                            article.OrderByDescending(s => s.ThemeId);
                break;
                case "memberName":
                article = searchDTO.sortType == "asc" ? article.OrderBy(s => s.MemberName) :
                                                            article.OrderByDescending(s => s.MemberName);
                break;
                case "title":
                article = searchDTO.sortType == "asc" ? article.OrderBy(s => s.Title) :
                                                            article.OrderByDescending(s => s.Title);
                break;
                case "postDate":
                article = searchDTO.sortType == "asc" ? article.OrderBy(s => s.PostDate) :
                                                            article.OrderByDescending(s => s.PostDate);
                break;
                case "replyCount":
                article = searchDTO.sortType == "asc" ? article.OrderBy(s => s.ReplyCount) :
                                                            article.OrderByDescending(s => s.ReplyCount);
                break;
                case "lock":
                article = searchDTO.sortType == "asc" ? article.OrderBy(s => s.Lock) :
                                                            article.OrderByDescending(s => s.Lock);
                break;
                default:
                article = searchDTO.sortType == "asc" ? article.OrderBy(s => s.ArticleId) :
                                                            article.OrderByDescending(s => s.ArticleId);
                break;
            }

            return article;
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleId == id);
        }
    }
}
