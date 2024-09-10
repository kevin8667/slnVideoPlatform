using Dapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VdbAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class FourmImgController : ControllerBase {
        // GET: api/<fourmImgController>
        private readonly IWebHostEnvironment _web;
        private readonly string? _connection;
        public FourmImgController(IWebHostEnvironment environment,IConfiguration configuration)
        {
            _web = environment;
            _connection = configuration.GetConnectionString("VideoDB");
        }
        [HttpGet("UPDATEReplyCount")]
        public IActionResult UPDATEReplyCount()
        {
            try {
                using var con = new SqlConnection(_connection);
                // 查找 ReplyCount 不等於實際 Post 數量的文章，並更新它們
                var sql = @"
            UPDATE Article
            SET ReplyCount = (
                SELECT COUNT(*)
                FROM Post
                WHERE Post.ArticleID = Article.ArticleID
            )
            WHERE Article.ArticleID IN (
                SELECT Article.ArticleID
                FROM Article
                JOIN Post 
                ON Post.ArticleID = Article.ArticleID
                GROUP BY Article.ArticleID, Article.ReplyCount
                HAVING Article.ReplyCount != COUNT(Post.PostID)
            );";

                var result = con.Execute(sql);

                return Ok(result > 0 ? $"{result} 篇文章的回覆數已更新" : "無需更新");
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

        [HttpGet("PostLikeNull")]
        public IActionResult ChangePostLikeNull()
        {
            string sql = @"update Post
                          set LikeCount = 0,DislikeCount = 0
                          where LikeCount is null or DislikeCount is null";
            using var con = new SqlConnection(_connection);
            var result = con.Execute(sql);
            return Ok(result > 0 ? $"{result} 篇回文的讚跟喜歡已更新" : "無需更新");

        }

        [HttpGet("ArticleLikeNull")]
        public IActionResult ChangeArticleLikeNull()
        {
            string sql = @" update Article
                          set LikeCount = 0,DislikeCount = 0
                          where LikeCount is null or DislikeCount is null";
            using var con = new SqlConnection(_connection);
            var result = con.Execute(sql);
            return Ok(result > 0 ? $"{result} 篇文章的讚跟喜歡已更新" : "無需更新");
        }
       
        public class UserInfo {
            public IFormFile? UserPhoto {
                get;
                set;
            }
        }
    }
}