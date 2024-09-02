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
    public class PostsController : ControllerBase {
        private readonly VideoDBContext _context;
        private readonly string? _connection;

        public PostsController(VideoDBContext context,IConfiguration configuration)
        {
            _context = context;
            _connection = configuration.GetConnectionString("VideoDB");
        }

        // GET: api/Posts
        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetPosts()
        {
            return _context.Posts;
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetPost(int id)
        {
            var sql = new StringBuilder(@"select p.*,m.NickName FROM Post p 
                        join MemberInfo m
                        on m.MemberID = p.PosterID
                        where ArticleID = @Id");
            using var con = new SqlConnection(_connection);

            var post = await con.QueryAsync<PostDTO>(sql.ToString(),new {
                Id = id
            });

            if(post == null) {
                return NotFound();
            }

            return Ok(post.ToList());
        }

        // PUT: api/Posts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id,Post post)
        {
            if(id != post.PostId) {
                return BadRequest();
            }

            _context.Entry(post).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException) {
                if(!PostExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Posts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPost",new {
                id = post.PostId
            },post);
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if(post == null) {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
