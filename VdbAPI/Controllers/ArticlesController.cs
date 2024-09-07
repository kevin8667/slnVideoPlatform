using Dapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System.Text;

using VdbAPI.DTO;
using VdbAPI.Models;

namespace VdbAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase {
        private readonly VideoDBContext _context;
        private readonly string? _connection;
        public ArticlesController(VideoDBContext context,IConfiguration configuration)
        {
            _context = context;
            _connection = configuration.GetConnectionString("VideoDB");
        }

        // GET: api/Articles
        [HttpGet]
        public ActionResult<IEnumerable<Theme>> GetThemes()
        {
            return _context.Themes;
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleView>> GetArticle(int id)
        {
            const string sql = "SELECT * FROM ArticleView WHERE ArticleId = @Id ";

            using var connection = new SqlConnection(_connection);
            var article = await connection.QueryFirstOrDefaultAsync<ArticleView>(sql,new {
                Id = id
            });
            if(article == null) {
                return NotFound();
            }
            return Ok(article);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchArticle(int id,ArtcleUpdate artcleUpdate)
        {
            if(artcleUpdate.ArticleContent.Length < 20)
                return BadRequest("文章內容長度必須至少 20 個字元。");
            var sql = "UPDATE Article SET ArticleContent = @ArticleContent WHERE ArticleId = @Id";
            using var con = new SqlConnection(_connection);
            var rowsAffected = await con.ExecuteAsync(sql,new {
                artcleUpdate.ArticleContent,
                Id = id
            });

            if(rowsAffected == 0) {
                return NotFound();
            }
            return Ok();
        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if(article == null) {
                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<forumDto>> LoadIndex(forumDto searchDTO)
        {
            try {
                using var connection = new SqlConnection(_connection);
                var sql = new StringBuilder(@"select * from ArticleView WHERE 1=1 and lock = 1");
                // 篩選條件
                if(searchDTO.categoryId != 0) {
                    sql.Append(" AND ThemeId = @CategoryId");
                }
                // 關鍵字篩選

                if(!string.IsNullOrEmpty(searchDTO.keyword)) {
                    sql.Append(" AND (Title LIKE @Keyword OR ArticleContent LIKE @Keyword OR NickName LIKE @Keyword)");
                }
                // 排序

                // 計算總筆數
                var countSql = $"SELECT COUNT(1) FROM ({sql.ToString()}) AS CountQuery";
                var dataCount = await connection.ExecuteScalarAsync<int>(countSql,new {
                    CategoryId = searchDTO.categoryId,
                    Keyword = $"%{searchDTO.keyword}%"
                });

                // 排序條件
                sql.Append(" Order By UpdateDate Desc"); // 根據你的排序需求修改

                // 分頁
                int pageSize = searchDTO.pageSize ?? 10;
                int page = searchDTO.page ?? 1;
                int totalPages = (int)Math.Ceiling((decimal)dataCount / pageSize);

                sql.Append(" OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");


                var articles = await connection.QueryAsync<ArticleView>(sql.ToString(),new {
                    CategoryId = searchDTO.categoryId,
                    Keyword = $"%{searchDTO.keyword}%",
                    Offset = (page - 1) * pageSize,
                    PageSize = pageSize
                });

                // 跳過指定頁數的資料並取出當前頁面的資料

                // 準備回傳的 DTO
                var pagingDTO = new ForumPagingDTO {
                    TotalCount = dataCount,
                    TotalPages = totalPages,
                    ForumResult = articles.Take(pageSize).ToList(),
                };

                return Ok(pagingDTO); // 返回 OK 和 DTO
            }
            catch(Exception ex) {
                // 返回 500 錯誤和錯誤信息
                return StatusCode(StatusCodes.Status500InternalServerError,"錯誤原因:" + ex.Message);
            }
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleId == id);
        }
    }

    public class ArtcleUpdate {
        public required string ArticleContent {
            get;
            set;
        }
        public  int? ThemeId {
            get;
            set;
        }
        public  int? Author {
            get;
            set;
        }
    }
}
