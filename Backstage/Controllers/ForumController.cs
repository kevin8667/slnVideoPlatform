using Microsoft.AspNetCore.Mvc;

namespace Backstage.Controllers {
    public class ForumController : Controller {
        public IActionResult Index()
        {
            return View();
        }
    }
}
