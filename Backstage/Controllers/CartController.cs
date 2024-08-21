using Backstage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backstage.Controllers
{
    public class CartController : Controller
    {
        private readonly VideoDBContext _dbContext;

        public CartController(VideoDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
