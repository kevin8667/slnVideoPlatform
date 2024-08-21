using Backstage.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Backstage.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly VideoDBContext _dbContext;

        public HomeController(ILogger<HomeController> logger, VideoDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Test() 
        {
            return View(_dbContext.VideoLists);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult PlanLists()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
