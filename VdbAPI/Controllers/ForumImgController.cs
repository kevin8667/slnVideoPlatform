using Dapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System.Text;

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
        [HttpGet("ArticleContent")]
        public async Task<IActionResult> NullArticleAsync()
        {
            try {
                using var con = new SqlConnection(_connection);
                await con.OpenAsync(); // 顯式打開連接
                using var transaction = await con.BeginTransactionAsync();
                try {
                    var sql = @"Update Article
                            Set ArticleContent = '<h1>清新多多綠好喝</h1><p><span class=""ql-size-large"">雞腿也讚!</span></p>
                            <p><br></p>'
                            where ArticleContent is null";
                    var result = await con.ExecuteAsync(sql,transaction: transaction);

                    await transaction.CommitAsync();
                    return Ok($"更新{result}筆");
                }
                catch {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch(Exception ex) {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpGet("UPDATEReplyCount")]
        public async Task<IActionResult> UPDATEReplyCountAsync()
        {
            try {
                using var con = new SqlConnection(_connection);
                await con.OpenAsync(); // 顯式打開連接
                using var transaction = await con.BeginTransactionAsync();
                try {
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

                    var result = await con.ExecuteAsync(sql,transaction: transaction);

                    await transaction.CommitAsync();
                    return Ok(result > 0 ? $"{result} 篇文章的回覆數已更新" : "無需更新");
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
                Console.WriteLine("Image folder path: " + path);  // 输出物理路径
                // 生成文件名
                string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(user.UserPhoto.FileName);
                string fullPath = Path.Combine(path,newFileName);  // 完整路徑


                // 寫入文件
                using(var filestream = new FileStream(fullPath,FileMode.Create)) {
                    user.UserPhoto.CopyTo(filestream);
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

        [HttpGet("UpdateLikeCounts")]
        public async Task<IActionResult> UpdateLikeCountsAsync()
        {
            try {
                using var con = new SqlConnection(_connection);
                await con.OpenAsync(); // 顯式打開連接
                using var transaction = await con.BeginTransactionAsync();
                try {
                    // 更新文章的讚和不喜歡計數
                    string articleSql = @"UPDATE Article
                                        SET LikeCount = COALESCE(LikeCount, 0),
                                            DislikeCount = COALESCE(DislikeCount, 0)
                                        WHERE LikeCount IS NULL OR DislikeCount IS NULL";
                    var articleResult = await con.ExecuteAsync(articleSql,transaction: transaction);

                    // 更新帖子的讚和不喜歡計數
                    string postSql = @"UPDATE Post
                                    SET LikeCount = COALESCE(LikeCount, 0),
                                        DislikeCount = COALESCE(DislikeCount, 0)
                                    WHERE LikeCount IS NULL OR DislikeCount IS NULL";
                    var postResult = await con.ExecuteAsync(postSql,transaction: transaction);

                    await transaction.CommitAsync();

                    return Ok($"已更新 {articleResult} 篇文章和 {postResult} 篇回文的讚和不喜歡計數");
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

        public class UserInfo {
            public IFormFile? UserPhoto {
                get;
                set;
            }
        }
    }
}