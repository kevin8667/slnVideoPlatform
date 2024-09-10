using Microsoft.AspNetCore.Mvc;
using System.Linq;
//using VdbAPI.Data;
using VdbAPI.Models;

namespace VdbAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowtimesController : ControllerBase
    {
        private readonly VideoDBContext _context;
        public ShowtimesController(VideoDBContext context)
        {
            _context = context;
        }
        [HttpGet("nowshowing/{videoId}")] // 設定 API 路徑為 "api/showtimes/nowshowing/{videoId}"，{videoId} 是路徑中的參數。
        public IActionResult GetCinemasByMovie(int videoId) // 定義一個方法來處理 GET 請求，接收電影的 videoId 作為參數。
        {
            // 使用 LINQ 查詢，從 NowShowingTheaters 表中找到符合指定 videoId 的所有影院。
            var cinemas = (from nst in _context.NowShowingTheaters
                           join cinema in _context.Cinemas on nst.CinemaId equals cinema.CinemaId 
                           where nst.VideoId == videoId // 過濾條件，查找符合指定 VideoID 的紀錄。
                           select new 
                           {
                               cinema.CinemaId, 
                               cinema.CinemaName, 
                               cinema.CinemaAddress  
                           }).ToList(); // 執行查詢並轉換成列表。

            
            if (!cinemas.Any()) // 如果查詢結果為空
                return NotFound("沒有找到相關的上映影院"); 

            // 如果有查詢結果，返回 200 狀態碼以及查詢結果的列表。
            return Ok(cinemas); // 返回 200 OK，並將查詢結果 cinemas 以 JSON 格式返回給用戶端。
        }


        // 根據選擇的影院取得放映時間
        [HttpGet("cinema/{cinemaId}")]
        public IActionResult GetShowtimesByCinema(int cinemaId)
        {
            var showtimes = (from showtime in _context.Showtimes
                             join hall in _context.Halls on showtime.HallsId equals hall.HallsId
                             where hall.CinemaId == cinemaId
                             select new
                             {
                                 showtime.ShowtimeId,
                                 showtime.ShowTimeDatetime,
                                 hall.HallsName
                             }).ToList();

            if (!showtimes.Any())
                return NotFound("沒有找到該影院的放映時間");

            return Ok(showtimes);
        }
    }
}
