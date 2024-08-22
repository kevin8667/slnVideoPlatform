using Backstage.Models;
using Backstage.Models.Cart_DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

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
        public IActionResult VideoPlanLists() {
           return View();
        }

        [HttpGet]
        public async Task<IActionResult> showPlanLists()
        {
            // 從資料庫中抓取資料
            var PlanLists = await _dbContext.PlanLists.ToListAsync();
            return Json(PlanLists);
        }

        [HttpPost]
        public async Task<IActionResult> showVideoPlanLists()
        {
            // 從資料庫中抓取資料
            var ViedoPlanLists = await _dbContext.ViedoPlanLists
                .Include(db => db.Plan)
                .Include(db => db.Video)
                .Select(db => new {
                    ViedoPlanId=db.videoPlanId,
                    PlanId = db.planId,
                    ViedoId = db.videoId,
                    VideoName = db.Video.VideoName,
                    PlanName = db.Plan.PlanName
                })
                .ToListAsync();
            


            //var Plans = sVP.PlanListId == 0 ? _dbContext.ViedoPlanLists : _dbContext.ViedoPlanLists.Where(a => a.planId == sVP.PlanListId);
            ////關鍵字搜尋
            //if (!string.IsNullOrEmpty(sVP.keyword))
            //{
            //    Plans = Plans.Where(a => a.Plan.PlanName.Contains(sVP.keyword));
            //}
            //csVideoPlanLists x = new csVideoPlanLists();
            //x.ViedoPlanList = Plans.ToList();
            //return Json(x);


            return Json(ViedoPlanLists);
        }
        [HttpPost]
        public IActionResult searchVideoPlans([FromBody]SearchVideoPlanLists sVP) 
        {
            //根據PlanListsId抓資料
            var Plans =  sVP.PlanListId == 0? _dbContext.ViedoPlanLists : _dbContext.ViedoPlanLists.Where(a=>a.planId == sVP.PlanListId);
            //關鍵字搜尋
            if (!string.IsNullOrEmpty(sVP.keyword)) 
            {
                Plans = Plans.Where(a => a.Plan.PlanName.Contains(sVP.keyword));
            }
            csVideoPlanLists x = new csVideoPlanLists();
            x.ViedoPlanList = Plans.ToList();
            return Json(x);

            //IQueryable<ViedoPlanList> vpQuery = _dbContext.ViedoPlanLists;
            //if (!string.IsNullOrEmpty(sVP.keyword))
            //{
            //    vpQuery = vpQuery.Where(a => a.Plan.PlanName.Contains(sVP.keyword));
            //}
            //csVideoPlanLists x = new csVideoPlanLists();

        }
    }
}
