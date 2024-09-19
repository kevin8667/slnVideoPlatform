using Dapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using VdbAPI.DTO;
using VdbAPI.Models;

namespace VdbAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ChatRoomsController : ControllerBase {
        private readonly VideoDBContext _context;
        private readonly string? _connection;


        public ChatRoomsController(VideoDBContext context,IConfiguration configuration)
        {
            _context = context;
            _connection = configuration.GetConnectionString("VideoDB");
        }

        // GET: api/ChatRooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatroomDTO>>> GetChatRooms()
        {
            var sql = @"  SELECT TOP 50
                    c.SenderId,
                    c.ChatMessage,c.SendTime,
                    m.Nickname 
                FROM ChatRoom c
                LEFT JOIN MemberInfo m ON c.SenderId = m.MemberId
                ORDER BY c.SendTime DESC";
            try {
                using var con = new SqlConnection(_connection);
                var chatrooms = await con.QueryAsync<ChatroomDTO>(sql);
                return Ok(chatrooms);
            }
            catch(Exception ex) {
                return StatusCode(500,"內部服務器錯誤" + ex.Message);
            }
        }

        //GET: api/ChatRooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChatRoom>> GetChatRoom(int id)
        {
            var chatRoom = await _context.ChatRooms.FindAsync(id);

            if (chatRoom == null)
            {
                return NotFound();
            }

            return chatRoom;
        }

        // DELETE: api/ChatRooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChatRoom(int id)
        {
            var chatRoom = await _context.ChatRooms.FindAsync(id);
            if(chatRoom == null) {
                return NotFound();
            }

            _context.ChatRooms.Remove(chatRoom);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChatRoomExists(int id)
        {
            return _context.ChatRooms.Any(e => e.ChatRoomId == id);
        }
    }
}
