using Dapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System.Text;
using System.Data;
using System.Data.Common;

namespace VdbAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ForumImgController : ControllerBase {
        // GET: api/<forumImgController>
        private readonly IWebHostEnvironment _web;
        private readonly string? _connection;
        public ForumImgController(IWebHostEnvironment environment,IConfiguration configuration)
        {
            _web = environment;
            _connection = configuration.GetConnectionString("VideoDB");
        }
        [HttpGet("ExecuteDatabaseOperations")]
        public async Task<IActionResult> ExecuteDatabaseOperationsAsync()
        {
            var results = new List<string>();
            try {
                using var con = new SqlConnection(_connection);
                await con.OpenAsync();
                using var transaction = await con.BeginTransactionAsync();
                try {
                    // NullArticle 操作
                    await NullArticle(results,con,transaction);

                    // UPDATEReplyCount 操作
                    await UPDATEReplyCount(results,con,transaction);

                    // UpdateLikeCounts 操作
                    await UpdateLikeCounts(results,con,transaction);

                    await transaction.CommitAsync();
                    return Ok(string.Join("\n",results));
                }
                catch {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch(Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,"錯誤原因:" + ex.Message);
            }
        }

        [HttpPost("create-user-reactions-table")]
        public IActionResult CreateUserReactionsTable()
        {
            string sql = @"
           IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'UserReactions')
            BEGIN
                CREATE TABLE UserReactions (
                    CountId INT PRIMARY KEY IDENTITY(1,1),
                    MemberId INT NOT NULL,
                    ArticleId INT NOT NULL,
                    ReactionType SMALLINT NOT NULL, -- 1 for like, -1 for dislike
                    CONSTRAINT FK_UserReactions_MemberInfo FOREIGN KEY (MemberId) REFERENCES MemberInfo(MemberId),
                    CONSTRAINT FK_UserReactions_Article FOREIGN KEY (ArticleId) REFERENCES Article(ArticleID),
                    CONSTRAINT UQ_UserReactions_MemberArticle UNIQUE (MemberId, ArticleId)
                )
            END";

            try {
                using(var connection = new SqlConnection(_connection)) {
                    connection.Execute(sql);
                }
                return Ok("UserReactions 表已成功創建");
            }
            catch(Exception ex) {
                return StatusCode(500,$"創建表時發生錯誤: {ex.Message}");
            }
        }
        private async Task NullArticle(List<string> results,SqlConnection con,DbTransaction transaction)
        {
            var sql = @"Update Article
                        Set ArticleContent = '<h1>清新多多綠好喝</h1><p><span class=""ql-size-large"">雞腿也讚!</span></p>
                        <p><br></p>'
                        where ArticleContent is null";
            var result = await con.ExecuteAsync(sql, transaction: transaction);
            results.Add($"更新空文章內容：{result}筆");
        }

        private async Task UPDATEReplyCount(List<string> results,SqlConnection con,DbTransaction transaction)
        {
            var sql = @"UPDATE Article
                        SET ReplyCount = (
                            SELECT COUNT(*)
                            FROM Post
                            WHERE Post.ArticleID = Article.ArticleID
                        )
                        WHERE Article.ArticleID IN (
                        SELECT Article.ArticleID
                        FROM Article
                        LEFT JOIN Post 
                        ON Post.ArticleID = Article.ArticleID
                        GROUP BY Article.ArticleID, Article.ReplyCount
                        HAVING Article.ReplyCount != COUNT(Post.ArticleID)
                        )";
            var result = await con.ExecuteAsync(sql, transaction: transaction);
            results.Add(result > 0 ? $"{result} 篇文章的回覆數已更新" : "文章回覆數無需更新");
        }

        private async Task UpdateLikeCounts(List<string> results,SqlConnection con,DbTransaction transaction)
        {
            var updateArticleLikesSql = @"UPDATE Article
                                              SET LikeCount = COALESCE(LikeCount, 0),
                                                  DislikeCount = COALESCE(DislikeCount, 0)
                                              WHERE LikeCount IS NULL OR DislikeCount IS NULL";
            var updateArticleLikesResult = await con.ExecuteAsync(updateArticleLikesSql,transaction: transaction);

            var updatePostLikesSql = @"UPDATE Post
                                           SET LikeCount = COALESCE(LikeCount, 0),
                                               DislikeCount = COALESCE(DislikeCount, 0)
                                           WHERE LikeCount IS NULL OR DislikeCount IS NULL";
            var updatePostLikesResult = await con.ExecuteAsync(updatePostLikesSql,transaction: transaction);

            results.Add($"已更新 {updateArticleLikesResult} 篇文章和 {updatePostLikesResult} 篇回文的讚和不喜歡計數");
        }

        [HttpPost]
        public IActionResult UploadUserPhoto(UserInfo user)
        {
            if(user.UserPhoto == null || user.UserPhoto.Length == 0)
                return BadRequest(new {
                    error = "圖片未填入"
                });

            try {
                // 將圖片保存到 Img 文件夾中
                string? rootPath = _web.ContentRootPath;
                if(string.IsNullOrEmpty(rootPath)) {
                    return NotFound(new {
                        error = "目標路徑不存在"
                    });
                }
                string path = Path.Combine(rootPath,"img");
                if(!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }
                // 生成文件名
                string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(user.UserPhoto.FileName);
                string fullPath = Path.Combine(path,newFileName);  // 完整路徑


                // 寫入文件
                using(var livestream = new FileStream(fullPath,FileMode.Create)) {
                    user.UserPhoto.CopyTo(livestream);
                }

                // 返回可用於前端的 URL

                return Ok(new {
                    filePath = $"https://localhost:7193/img/{newFileName}"
                });
            }
            catch(Exception ex) {
                return StatusCode(500,new {
                    error = "文件保存時發生錯誤: " + ex.Message
                });
            }
        }

        public class UserInfo {
            public IFormFile? UserPhoto {
                get;
                set;
            }
        }
    }
}