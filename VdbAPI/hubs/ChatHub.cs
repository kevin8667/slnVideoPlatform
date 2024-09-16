using Dapper;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using NuGet.Protocol.Plugins;

using VdbAPI.DTO;
using VdbAPI.Models;

namespace VdbAPI.hubs {
    public class ChatHub : Hub {
        // 客戶端可以調用此方法來發送訊息
        private readonly VideoDBContext _context;
        private readonly string? _connection;

        public ChatHub(VideoDBContext context,IConfiguration configuration)
        {
            _context = context;
            _connection = configuration.GetConnectionString("VideoDB");
        }
        public async Task SendMessage(ChatroomDTO chatDTO)
        {
            var sql = @"Insert Chatroom(SenderId,SendTime,ChatMessage)
                        values(@SenderId,@SendTime,@ChatMessage)";
            using var con = new SqlConnection(_connection);

            await con.ExecuteAsync(sql,new {
                SendTime = chatDTO.Sendtime,
                SenderId = chatDTO.SenderId,
                ChatMessage = chatDTO.ChatMessage
            });
            chatDTO.IsMined = true;
            await Clients.All.SendAsync("ReceiveMessage",chatDTO);
        }

    }
}
