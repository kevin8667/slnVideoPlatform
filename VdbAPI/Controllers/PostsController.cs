using Dapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System.Text;

using VdbAPI.DTO;
using VdbAPI.Models;

namespace VdbAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase {
        private readonly VideoDBContext _context;
        private readonly string? _connection;

        public PostsController(VideoDBContext context,IConfiguration configuration)
        {
            _context = context;
            _connection = configuration.GetConnectionString("VideoDB");
        }
        [HttpPost("React")]
        public async Task<IActionResult> React(LikeDTO likeDTO)
        {
            var post = await _context.Posts.FindAsync(likeDTO.ContentId);
            if(post == null)
                return NotFound("回文不存在");

            using var connection = new SqlConnection(_connection);

            try {
                // SQL 處理 UserReactions
                var postUserReaction = await _context.PostUserReactions
                            .FirstOrDefaultAsync(ur => ur.PostId == likeDTO.ContentId);
                string sql = @"select articleId from post where PostId = @PostId";
                int? articleId = await connection.QueryFirstOrDefaultAsync<int?>(sql,new {
                    PostId = likeDTO.ContentId
                });

                if(!articleId.HasValue)
                    return NotFound("回文不存在");

                if(likeDTO.ReactionType.HasValue) {
                    if(postUserReaction == null) {
                        postUserReaction = new PostUserReaction {
                            MemberId = likeDTO.MemberId,
                            ArticleId = articleId.Value,
                            ReactionType = likeDTO.ReactionType.Value,
                            PostId = likeDTO.ContentId,
                        };
                        // 沒有紀錄就加一筆
                        _context.PostUserReactions.Add(postUserReaction);
                    }
                    else {
                        postUserReaction.ReactionType = likeDTO.ReactionType.Value; //有紀錄就改值
                    }
                }
                else {
                    if(postUserReaction != null) {
                        _context.PostUserReactions.Remove(postUserReaction); //被取消就刪掉舊紀錄
                    }
                }

                await _context.SaveChangesAsync();

                var sqlGetCounts = @"
                SELECT 
                (SELECT COUNT(1) FROM PostUserReactions WHERE PostId = @PostId AND ReactionType = 1) AS LikeCount,
                (SELECT COUNT(1) FROM PostUserReactions WHERE PostId = @PostId AND ReactionType = 0) AS DislikeCount";

                var counts = await connection.QueryFirstAsync(sqlGetCounts,new {
                    PostId = likeDTO
                .ContentId
                });

                // 更新文章的計數
                post.LikeCount = counts.LikeCount;
                post.DislikeCount = counts.DislikeCount;
                await _context.SaveChangesAsync();

                return Ok(counts);
            }
            catch(Exception ex) {
                return StatusCode(500,"錯誤原因: " + ex.Message);
            }
        }

        // GET: api/Posts
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var sql = @"select * from post where postId = @Id";
            using var con = new SqlConnection(_connection);
            var post = await con.QueryFirstOrDefaultAsync<Post>(sql,new {
                Id = id
            });
            if(post == null)
                return NotFound();
            return Ok(post);
        }

        // GET: api/Posts/5
        [HttpGet("all/{articleId}")]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetPosts(int articleId)
        {
            var sql = new StringBuilder(@"select p.*,m.NickName FROM Post p 
                        join MemberInfo m
                        on m.MemberID = p.PosterID
                        where ArticleID = @Id");
            using var con = new SqlConnection(_connection);

            var post = await con.QueryAsync<PostDTO>(sql.ToString(),new {
                Id = articleId
            });

            if(post == null) {
                return NotFound();
            }

            return Ok(post.ToList());
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePost(int id,PostUpdate postUpdate)
        {
            if(!PostExists(id))
                return BadRequest(new {
                    Error = "無此回文的ID"
                });

            if(postUpdate.PostContent.Length < 20) {
                return BadRequest(new {
                    錯誤 = "回文字數過少"
                });
            }
            try {
                var sql = "UPDATE Post SET PostContent = @PostContent WHERE PostId = @Id";
                using var con = new SqlConnection(_connection);
                var rowsAffected = await con.ExecuteAsync(sql,new {
                    postUpdate.PostContent,
                    Id = id
                });

                if(rowsAffected == 0) {
                    return NotFound();
                }

                return Ok(new {
                    OK = "修改回文成功",
                });
            }
            catch(Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,new {
                    Message = "內部服務器錯誤",
                    Details = ex.Message
                });
            }

        }

        // POST: api/Posts
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(PostDTO postDTO)
        {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if(postDTO == null) {
                return NotFound(new {
                    error = "找不到post"
                });
            }
            try {

                var post = new Post {
                    ArticleId = postDTO.ArticleId,
                    Lock = true,
                    PostContent = postDTO.PostContent,
                    PostDate = DateTime.UtcNow,
                    PosterId = postDTO.PosterId,
                    PostImage = "",
                    LikeCount = 0,
                    DislikeCount = 0,

                };
                _context.Posts.Add(post);

                var article = await _context.Articles.FirstOrDefaultAsync(c => c.ArticleId == post.ArticleId);
                if(article != null) {

                    article.ReplyCount++;                  // 回覆次數增加 1
                    article.UpdateDate = DateTime.UtcNow;
                    _context.Articles.Update(article);
                }

                await _context.SaveChangesAsync();

                return Ok(new {
                    message = "新增回文成功"
                });
            }
            catch(Exception ex) {
                return StatusCode(500,new {
                    error = "發生例外的錯誤:" + ex.Message,
                });
            }
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if(post == null) {
                return NotFound(new {
                    error = "沒有符合此ID的POST"
                });
            }

            _context.Posts.Remove(post);
            var article = await _context.Articles.FirstOrDefaultAsync(c => c.ArticleId == post.ArticleId);
            if(article != null) {
                article.ReplyCount--;
                _context.Articles.Update(article);
            }
            await _context.SaveChangesAsync();

            return Ok(new {
                success = "已完成刪除作業"
            });
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }

    public class PostUpdate {
        public required string PostContent {
            get; set;
        }
    }
}
