using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VdbAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VdbAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly VideoDBContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TestController(VideoDBContext dBContext,IWebHostEnvironment webHostEnvironment) 
        {
            _dbContext = dBContext;
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<VideoList>>> GetProducts()
        {
            return await _dbContext.VideoLists.ToListAsync();
        }

    }
}
