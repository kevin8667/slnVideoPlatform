using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backstage.Models;
using Backstage.Models.DTO;

namespace Backstage.Controllers {
    public class ArticlesController : Controller {
        private readonly VideoDBContext _dbContext;

        public ArticlesController(VideoDBContext context)
        {
            _dbContext = context;
        }

        // GET: Articles
        public async Task<IActionResult> Index()
        {


            ViewBag.Theme = _dbContext.Themes;
            var videoDBContext = _dbContext.Articles.Include(a => a.Member).Include(a => a.Theme);
            return View(await videoDBContext.ToListAsync());
        }

        [HttpPost]
        public IActionResult Index([FromBody] ForumDTO searchDTO)
        {
            try {
                var spot = searchDTO.categoryId == 0 ? _dbContext.Articles : _dbContext.Articles
                                                                                .Where(c => c.ThemeId == searchDTO.categoryId);
                if(!string.IsNullOrEmpty(searchDTO.keyword))
                    spot = _dbContext.Articles.Where(c => c.Title.Contains(searchDTO.keyword) ||
                    c.ArticleContent.Contains(searchDTO.keyword));

                switch(searchDTO.sortBy) {
                    case "spotTitle":
                    spot = searchDTO.sortType == "asc" ? spot.OrderBy(s => s.Title) :
                                                         spot.OrderByDescending(s => s.Title);
                    break;
                    default:
                    spot = searchDTO.sortType == "asc" ? spot.OrderBy(s => s.ArticleId) :
                                                         spot.OrderByDescending(s => s.ArticleId);
                    break;
                }
                // 總筆數/一頁大小並無條件進位  
                int dataCount = spot.Count();

                int PagesSize = searchDTO.pageSize ?? 9;
                int Page = searchDTO.page ?? 1;
                int TotalPages = (int)Math.Ceiling(((decimal)dataCount / PagesSize));
                //跳過幾筆資料 意思是指定頁-1後*一頁大小
                spot = spot.Skip((Page - 1) * PagesSize).Take(PagesSize);
                ForumPagingDTO pagingDTO = new ForumPagingDTO();
                pagingDTO.TotalCount = dataCount;
                pagingDTO.TotalPages = TotalPages;
                pagingDTO.ForumResult = spot.ToList();
                return Json(pagingDTO);
            }
            catch(Exception ex) {
                throw new Exception(ex.Message,ex);
            }

        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null) {
                return NotFound();
            }

            var article = await _dbContext.Articles
                .Include(a => a.Member)
                .Include(a => a.Theme)
                .FirstOrDefaultAsync(m => m.ArticleId == id);
            if(article == null) {
                return NotFound();
            }
            return PartialView("_ArticleDetailsPartial ",article);

        }

        // GET: Articles/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_dbContext.MemberInfos,"MemberId",
                "MemberName");
            ViewData["ThemeId"] = new SelectList(_dbContext.Themes,"ThemeId",
                "ThemeName");
            return View();
        }

        // POST: Articles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArticleId,ThemeId,AuthorId," +
            "Title,ArticleContent,PostDate,UpdateDate,ReplyCount,Lock,ArticleImage")] Article article)
        {
            if(ModelState.IsValid) {
                _dbContext.Add(article);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_dbContext.MemberInfos,"MemberId",
                "MemberName",article.AuthorId);
            ViewData["ThemeId"] = new SelectList(_dbContext.Themes,"ThemeId",
                "ThemeName",article.ThemeId);
            return View(article);
        }

        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null) {
                return NotFound();
            }

            var article = await _dbContext.Articles.FindAsync(id);
            if(article == null) {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_dbContext.MemberInfos,
                "MemberId","MemberName",article.AuthorId);
            ViewData["ThemeId"] = new SelectList(_dbContext.Themes,
                "ThemeId","ThemeName",article.ThemeId);
            return View(article);
        }

        // POST: Articles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,[Bind("ArticleId,ThemeId," +
            "AuthorId,Title,ArticleContent,PostDate,UpdateDate,ReplyCount,Lock,ArticleImage")] Article article)
        {
            if(id != article.ArticleId) {
                return NotFound();
            }

            if(ModelState.IsValid) {
                try {
                    _dbContext.Update(article);
                    await _dbContext.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException) {
                    if(!ArticleExists(article.ArticleId)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_dbContext.MemberInfos,
                "MemberId","MemberName",article.AuthorId);
            ViewData["ThemeId"] = new SelectList(_dbContext.Themes,
                "ThemeId","ThemeName",article.ThemeId);
            return View(article);
        }

        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null) {
                return NotFound();
            }

            var article = await _dbContext.Articles
                .Include(a => a.Member)
                .Include(a => a.Theme)
                .FirstOrDefaultAsync(m => m.ArticleId == id);
            if(article == null) {
                return NotFound();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _dbContext.Articles.FindAsync(id);
            if(article != null) {
                _dbContext.Articles.Remove(article);
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _dbContext.Articles.Any(e => e.ArticleId == id);
        }
    }

    public class ForumPagingDTO {

        public int TotalCount {
            get;
            set;
        }
        public int TotalPages {
            get;
            set;
        }
        public List<Article> ForumResult {
            get;
            set;
        }
    }
}
