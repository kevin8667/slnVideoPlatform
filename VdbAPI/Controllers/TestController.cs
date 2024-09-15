//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using VdbAPI.Models;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace VdbAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TestController : ControllerBase
//    {
//        private readonly VideoDBContext _dbContext;
//        private readonly IWebHostEnvironment _webHostEnvironment;
//        public TestController(VideoDBContext dBContext,IWebHostEnvironment webHostEnvironment) 
//        {
//            _dbContext = dBContext;
//            _webHostEnvironment = webHostEnvironment;
//        }


//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<VideoList>>> GetProducts()
//        {
//            return await _dbContext.VideoLists.ToListAsync();
//        }

//    }
//}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VdbAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace VdbAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemaController : ControllerBase
    {
        private readonly VideoDBContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CinemaController(VideoDBContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        // Existing method to get video lists
        [HttpGet("videolists")]
        public async Task<ActionResult<IEnumerable<VideoList>>> GetVideoLists()
        {
            return await _dbContext.VideoLists.ToListAsync();
        }

        //[HttpGet("cinemas")]
        //public async Task<ActionResult<IEnumerable<Cinema>>> GetCinemas()
        //{
        //    var cinemas = await _dbContext.Cinemas
        //        ////.Include(c => c.Halls)
        //        //.Include(c => c.NowShowingTheaters)
        //        //.ToListAsync();

        //    // Use ReferenceHandler.Preserve to handle object cycles
        //    var options = new JsonSerializerOptions
        //    {
        //        ReferenceHandler = ReferenceHandler.Preserve,
        //        WriteIndented = true
        //    };

        //    return new JsonResult(cinemas, options);
        //}

    }
}

