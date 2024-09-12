using Dapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
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
        [HttpGet("React")]
        public async Task<IActionResult> React(int memberId,int articleId,int reactionType)
        {
            // 驗證 reactionType 參數
            if(reactionType != -1 && reactionType != 0 && reactionType != 1) {
                return BadRequest("無效的反應類型。必須是 -1, 0 或 1。");
            }

            // 驗證文章是否存在
            var article = await _context.Articles.FindAsync(articleId);
            if(article == null)
                return NotFound("文章不存在");

            using var connection = new SqlConnection(_connection);

            try {
                // SQL 處理 UserReactions
            var sqlUserReaction = @"
                IF EXISTS (SELECT 1 FROM UserReactions WHERE MemberId = @MemberId AND ArticleId = @ArticleId)
                BEGIN
                    UPDATE UserReactions SET ReactionType = @ReactionType 
                    WHERE MemberId = @MemberId AND ArticleId = @ArticleId;
                END
                ELSE
                BEGIN
                    INSERT INTO UserReactions (MemberId, ArticleId, ReactionType)
                    VALUES (@MemberId, @ArticleId, @ReactionType);
                END";

                await connection.ExecuteAsync(sqlUserReaction,new UserReaction{
                    MemberId = memberId,
                    ArticleId = articleId,
                    ReactionType = (short?)reactionType
                });

                var sqlGetCounts = @"
            SELECT 
                (SELECT COUNT(*) FROM UserReactions WHERE ArticleId = @ArticleId AND ReactionType = 1) AS LikeCount,
                (SELECT COUNT(*) FROM UserReactions WHERE ArticleId = @ArticleId AND ReactionType = -1) AS DislikeCount";

        var counts = await connection.QueryFirstAsync(sqlGetCounts, new { ArticleId = articleId });

        // 更新文章的計數
        article.LikeCount = counts.LikeCount;
        article.DislikeCount = counts.DislikeCount;
        await _context.SaveChangesAsync();

        return Ok(counts);
            }
            catch(Exception ex) {
                return StatusCode(500,"錯誤原因: " + ex.Message);
            }
        }



        // GET: api/Articles  取得主題標籤
        [HttpGet("Theme")]
        public ActionResult<IEnumerable<Theme>> GetThemes()
        {
            return _context.Themes;
        }

        // GET: api/Articles/5 取得文章內文
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleView>> GetArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if(article == null) {
                return NotFound(new {
                    message = "找不到指定的文章"
                });
            }

            const string sql = "SELECT * FROM ArticleView WHERE ArticleId = @Id ";

            using var connection = new SqlConnection(_connection);
            var articleView = await connection.QueryFirstOrDefaultAsync<ArticleView>(sql,new {
                Id = id
            });
            if(articleView == null) {
                return NotFound();
            }
            return Ok(articleView);
        }
        //新增
        [HttpPost]
        public async Task<IActionResult> CreateArticle(ArticleView articleView)
        {

            if(articleView == null) {
                return BadRequest(new {
                    error = "沒收到封包"
                });
            }
            if(!ModelState.IsValid) {
                return BadRequest("傳入值不符合規範" + ModelState);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try {
                var article = new Article {
                    AuthorId = articleView.AuthorId,
                    ThemeId = articleView.ThemeId,
                    Title = articleView.Title,
                    ArticleContent = articleView.ArticleContent,
                    Lock = true,
                    ArticleImage = "",
                    PostDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    ReplyCount = 0,
                    LikeCount = 0,
                    DislikeCount = 0,
                };

                _context.Articles.Add(article);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new {
                    OK = "新增文章成功"
                });
            }
            catch(Exception ex) {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError,"錯誤原因:" + ex.Message);

            }

        }
        //編輯
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchArticle(int id,ArticleUpdate articleUpdate)
        {
            if(articleUpdate.ArticleContent.Length < 10)
                return BadRequest("文章內容長度必須至少 10 個字元。");

            if(!ArticleExists(id))
                return NotFound(new {
                    Message = "未找到對應的文章 ID。"
                });

            var sql = @"UPDATE Article SET ArticleContent = @ArticleContent,Title = @Title,
                        ThemeId = @ThemeId WHERE ArticleId = @id";
            using var con = new SqlConnection(_connection);
            using var transaction = await con.BeginTransactionAsync();
            try {

                var rowsAffected = await con.ExecuteAsync(sql,new {
                    articleUpdate.ArticleContent,
                    articleUpdate.Title,
                    articleUpdate.ThemeId,
                    id,
                });

                if(rowsAffected == 0) {
                    return NotFound(new {
                        Message = "未找到要更新的文章。"
                    });

                }
                await transaction.CommitAsync();
                return Ok(new {
                    Message = "修改文章成功"
                });
            }
            catch(Exception ex) {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError,new {
                    Message = "內部服務器錯誤",
                    Details = ex.Message
                });
            }
        }

        // DELETE: api/Articles/5 刪除
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            var trans = await _context.Database.BeginTransactionAsync();
            if(article == null) {
                return NotFound();
            }
            try {
                await trans.CommitAsync();
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex) {
                await trans.RollbackAsync();
                return StatusCode(500,"錯誤原因: " + ex.Message);
            }
        }
        // 文章列表
        [HttpPost("LoadIndex")]
        public async Task<ActionResult<forumDto>> LoadIndex(forumDto searchDTO)
        {
            try {
                using var connection = new SqlConnection(_connection);
                var sql = new StringBuilder(@"select * from ArticleView WHERE 1=1 and [lock] = 1");
                // 篩選條件
                if(searchDTO.categoryId != 0) {
                    sql.Append(" AND ThemeId = @CategoryId");
                }
                // 關鍵字篩選
                // 定義變數
                string likePattern = $"%{searchDTO.keyword}%";
                // 根據條件添加搜尋詞
                if(!string.IsNullOrEmpty(searchDTO.keyword)) {
                    likePattern = $"%{searchDTO.keyword}%";
                    sql.Append(" AND ArticleContent LIKE @LikePattern OR Title LIKE @LikePattern OR NickName LIKE @LikePattern");
                }
               
                // 排序

                // 計算總筆數
                var countSql = $"SELECT COUNT(1) FROM ({sql}) AS CountQuery";
                var dataCount = await connection.ExecuteScalarAsync<int>(countSql,new {
                    CategoryId = searchDTO.categoryId,
                    LikePattern = likePattern
                });

                // 排序條件
                sql.Append(" ORDER BY [Lock] DESC, UpdateDate Desc"); // 根據你的排序需求修改

                // 分頁
                int pageSize = searchDTO.pageSize ?? 10;
                int page = searchDTO.page ?? 1;
                int totalPages = (int)Math.Ceiling((decimal)dataCount / pageSize);

                sql.Append(" OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");


                var articles = await connection.QueryAsync<ArticleView>(sql.ToString(),new {
                    CategoryId = searchDTO.categoryId,
                    LikePattern = likePattern,
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

    public class ArticleUpdate {
        public required string ArticleContent {
            get;
            set;
        }
        public required int ThemeId {
            get;
            set;
        }
        public int? AuthorID {
            get;
            set;
        }
        public required string Title {
            get;
            set;
        }
    }
}
