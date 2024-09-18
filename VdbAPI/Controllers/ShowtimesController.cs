using Microsoft.AspNetCore.Mvc;
using System.Linq;
using VdbAPI.DTOs;
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


        //訂單生成api
        [HttpPost]
        public IActionResult CreateReservation([FromBody] ReservationRequest request)
        {
            try
            {
                // 驗證輸入
                if (request == null || request.TicketCount <= 0)
                {
                    return BadRequest("無效的請求");
                }

                // 創建新的 ReservationDetail 資料，並保存 TicketCount
                var reservation = new ReservationDetail
                {
                    MemberId = request.MemberID,
                    ShowtimeId = request.ShowtimeID,
                    PurchaseDate = DateTime.Now,
                    Price = request.TotalPrice,
                    Status = "未付款", // 根據您的邏輯設置狀態
                    PaymentMethod = request.PaymentMethod,
                    CouponId = request.CouponID,
                    TicketCount = request.TicketCount  // 保存傳入的票數
                };

                // 將 ReservationDetail 寫入資料庫
                _context.ReservationDetails.Add(reservation);
                _context.SaveChanges();

                return Ok(new { ReservationID = reservation.ReservationId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.InnerException);
            }

        }


        //座位生成，並確保沒有重複
        [HttpPost("reservation/seats")]
        public IActionResult AssignSeats([FromBody] SeatAssignmentRequest request)
        {
            // 1. 驗證輸入
            if (request == null || request.TicketCount <= 0 || request.ReservationId <= 0)
            {
                return BadRequest("無效的請求");
            }

            // 2. 查詢該訂單詳細資料
            var reservation = _context.ReservationDetails.FirstOrDefault(r => r.ReservationId == request.ReservationId);
            if (reservation == null)
            {
                return NotFound("未找到該訂單");
            }

            // 3. 查詢該場次中已經被選取的座位
            var reservedSeats = _context.SessionSeats
                .Where(ss => ss.ShowtimeId == request.ShowtimeId && ss.SeatStatus == 2) // 確認該場次下已經被選取的座位
                .Select(ss => ss.SeatId)
                .ToList();

            // 4. 查詢尚未被選擇的座位（修改邏輯） 
            var availableSeats = _context.Seats
                .Where(s => !reservedSeats.Contains(s.SeatId) && s.HallsId == reservation.ShowtimeId) // 修改這裡，篩選出剩餘可用座位
                .OrderBy(s => s.SeatNumber) // 座位依照 SeatNumber 排序，確保連號分配
                .ToList();

            // 調試：輸出可用座位的數量和每個座位的 ID
            Console.WriteLine("Available Seats: " + availableSeats.Count);
            foreach (var seat in availableSeats)
            {
                Console.WriteLine("Seat ID: " + seat.SeatId);
            }

            // 5. 確認是否有足夠座位
            if (availableSeats.Count < request.TicketCount)
            {
                return BadRequest("無法找到足夠的座位");
            }

            // 6. 分配座位
            var assignedSeats = new List<int>();
            for (int i = 0; i < availableSeats.Count; i++)
            {
                if (assignedSeats.Count == request.TicketCount)
                {
                    break;
                }

                assignedSeats.Add(availableSeats[i].SeatId);

                // 7. 將座位記錄到 SessionSeats
                _context.SessionSeats.Add(new SessionSeat
                {
                    SeatId = availableSeats[i].SeatId,
                    ShowtimeId = request.ShowtimeId,
                    SeatStatus = 2,  // 標記座位為已選
                    ReservationId = request.ReservationId
                });
            }

            // 8. 更新資料庫
            _context.SaveChanges();

            // 9. 返回分配的座位
            return Ok(new
            {
                ReservationID = request.ReservationId,
                AssignedSeats = assignedSeats
            });
        }


        ////座位生成，並確保沒有重複
        //[HttpPost("reservation/seats")]
        //public IActionResult AssignSeats([FromBody] SeatAssignmentRequest request)
        //{



        //    // 1. 驗證輸入
        //    if (request == null || request.TicketCount <= 0 || request.ReservationId <= 0)
        //    {
        //        return BadRequest("無效的請求");
        //    }

        //    // 2. 查詢該訂單詳細資料
        //    var reservation = _context.ReservationDetails.FirstOrDefault(r => r.ReservationId == request.ReservationId);
        //    if (reservation == null)
        //    {
        //        return NotFound("未找到該訂單");
        //    }

        //    // 3. 查詢該場次中可用的座位
        //    var reservedSeats = _context.SessionSeats
        //        .Where(ss => ss.ShowtimeId == request.ShowtimeId && ss.SeatStatus == 2)
        //        .Select(ss => ss.SeatId)
        //        .ToList();

        //    var availableSeats = _context.Seats
        //        .Where(s => !reservedSeats.Contains(s.SeatId) && s.SeatId == reservation.ShowtimeId)
        //        .OrderBy(s => s.SeatNumber)
        //        .ToList();

        //    // 調試：輸出可用座位的數量和每個座位的 ID
        //    Console.WriteLine("Available Seats: " + availableSeats.Count);
        //    foreach (var seat in availableSeats)
        //    {
        //        Console.WriteLine("Seat ID: " + seat.SeatId);
        //    }



        //    if (availableSeats.Count < request.TicketCount)
        //    {
        //        return BadRequest("無法找到足夠的座位");
        //    }

        //    // 4. 分配連號座位
        //    var assignedSeats = new List<int>();
        //    for (int i = 0; i < availableSeats.Count; i++)
        //    {
        //        if (assignedSeats.Count == request.TicketCount)
        //        {
        //            break;
        //        }

        //        assignedSeats.Add(availableSeats[i].SeatId);

        //        // 5. 將座位記錄到 SessionSeats
        //        _context.SessionSeats.Add(new SessionSeat
        //        {
        //            SeatId = availableSeats[i].SeatId,
        //            ShowtimeId = request.ShowtimeId,
        //            SeatStatus = 2,  // 標記座位為已選
        //            ReservationId = request.ReservationId
        //        });
        //    }

        //    // 6. 更新資料庫
        //    _context.SaveChanges();

        //    // 7. 返回分配的座位
        //    return Ok(new
        //    {
        //        ReservationID = request.ReservationId,
        //        AssignedSeats = assignedSeats
        //    });
        //}

    }
}
