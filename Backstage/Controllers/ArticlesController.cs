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

        //GET: Articles
        public IActionResult Index()
        {


            ViewBag.Theme = _dbContext.Themes;
            var videoDBContext = _dbContext.ArticleViews;
            return View(videoDBContext);
        }

        [HttpPost]
        public IActionResult LoadIndex([FromBody] forumDto searchDTO)
        {
            try {
                var article = searchDTO.categoryId == 0 ? _dbContext.ArticleViews : _dbContext.ArticleViews
                                                                                .Where(c => c.ThemeId == searchDTO.categoryId);
                if(!string.IsNullOrEmpty(searchDTO.keyword))
                    article = _dbContext.ArticleViews.Where(c => c.Title.Contains(searchDTO.keyword) ||
                    c.ArticleContent.Contains(searchDTO.keyword));

                // 關鍵字篩選
                if(!string.IsNullOrEmpty(searchDTO.keyword)) {
                    article = article.Where(c => c.Title.Contains(searchDTO.keyword) ||
                                        c.ArticleContent.Contains(searchDTO.keyword) ||
                                        c.MemberName.Contains(searchDTO.keyword));
                }

                // 排序
                switch(searchDTO.sortBy) {
                    case "theme":
                    article = searchDTO.sortType == "asc" ? article.OrderBy(s => s.ThemeId) :
                                                                article.OrderByDescending(s => s.ThemeId);
                    break;
                    case "memberName":
                    article = searchDTO.sortType == "asc" ? article.OrderBy(s => s.AuthorId) :
                                                                article.OrderByDescending(s => s.AuthorId);
                    break;
                    case "title":
                    article = searchDTO.sortType == "asc" ? article.OrderBy(s => s.Title) :
                                                                article.OrderByDescending(s => s.Title);
                    break;
                    case "postDate":
                    article = searchDTO.sortType == "asc" ? article.OrderBy(s => s.PostDate) :
                                                                article.OrderByDescending(s => s.PostDate);
                    break;
                    case "replyCount":
                    article = searchDTO.sortType == "asc" ? article.OrderBy(s => s.ReplyCount) :
                                                                article.OrderByDescending(s => s.ReplyCount);
                    break;
                    case "lock":
                    article = searchDTO.sortType == "asc" ? article.OrderBy(s => s.Lock) :
                                                                article.OrderByDescending(s => s.Lock);
                    break;
                    default:
                    article = searchDTO.sortType == "asc" ? article.OrderBy(s => s.ArticleId) :
                                                                article.OrderByDescending(s => s.ArticleId);
                    break;
                }

                // 計算總筆數
                int dataCount = article.Count();

                // 分頁
                int PagesSize = searchDTO.pageSize ?? 10;
                int Page = searchDTO.page ?? 1;
                int TotalPages = (int)Math.Ceiling(((decimal)dataCount / PagesSize));

                // 跳過指定頁數的資料並取出當前頁面的資料
                var articles = article.Skip((Page - 1) * PagesSize).Take(PagesSize).ToList();

                // 準備回傳的 DTO
                ForumPagingDTO pagingDTO = new ForumPagingDTO {
                    TotalCount = dataCount,
                    TotalPages = TotalPages,
                    ForumResult = articles
                };

                return Json(pagingDTO);
            }
            catch(Exception ex) {
                throw new Exception(ex.Message,ex);
            }
        }



        //[HttpPost]
        //public IActionResult LoadIndex([FromBody] forumDto searchDTO)
        //{
        //    try {

        //         var article = searchDTO.categoryId == 0 ? _dbContext.Articles :
        //            _dbContext.Articles.Where(c => c.ThemeId == searchDTO.categoryId);

        //        if(!string.IsNullOrEmpty(searchDTO.keyword))
        //            article = _dbContext.Articles.Where(c => c.Title.Contains(searchDTO.keyword) ||
        //            c.ArticleContent.Contains(searchDTO.keyword)||
        //            c.Member.MemberName.Contains(searchDTO.keyword));

        //        switch(searchDTO.sortBy) {
        //            case "name":
        //            article = searchDTO.sortType == "asc" ? article.OrderBy(s => s.Member.MemberName) :
        //                                                 article.OrderByDescending(s => s.Member.MemberName);
        //            break;
        //            case "theme":
        //            article = searchDTO.sortType == "asc" ? article.OrderBy(s => s.Theme.ThemeName) :
        //                                                 article.OrderByDescending(s => s.Theme.ThemeName);
        //            break;
        //            case "title":
        //            article = searchDTO.sortType == "asc" ? article.OrderBy(s => s.Title) :
        //                                                 article.OrderByDescending(s => s.Title);
        //            break;
        //            case "postDate":
        //            article = searchDTO.sortType == "asc" ? article.OrderBy(s => s.PostDate) :
        //                                                 article.OrderByDescending(s => s.PostDate);
        //            break;
        //            case "replyCount":
        //            article = searchDTO.sortType == "asc" ? article.OrderBy(s => s.ReplyCount) :
        //                                                 article.OrderByDescending(s => s.ReplyCount);
        //            break;
        //            case "lock":
        //            article = searchDTO.sortType == "asc" ? article.OrderBy(s => s.Lock) :
        //                                                 article.OrderByDescending(s => s.Lock);
        //            break;
        //            default:
        //            article = searchDTO.sortType == "asc" ? article.OrderBy(s => s.ArticleId) :
        //                                                 article.OrderByDescending(s => s.ArticleId);
        //            break;
        //        }
        //        // 總筆數/一頁大小並無條件進位  
        //        int dataCount = article.Count();

        //        int PagesSize = searchDTO.pageSize ?? 10;
        //        int Page = searchDTO.page ?? 1;
        //        int TotalPages = (int)Math.Ceiling(((decimal)dataCount / PagesSize));
        //        //跳過幾筆資料 意思是指定頁-1後*一頁大小
        //        article = article.Skip((Page - 1) * PagesSize).Take(PagesSize);
        //        ForumPagingDTO pagingDTO = new ForumPagingDTO();
        //        pagingDTO.TotalCount = dataCount;
        //        pagingDTO.TotalPages = TotalPages;
        //        pagingDTO.ForumResult = article.ToList();
        //        return Json(pagingDTO);
        //    }
        //    catch(Exception ex) {
        //        throw new Exception(ex.Message,ex);
        //    }

        //}

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null) {
                return NotFound();
            }

            var article = await _dbContext.ArticleViews
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
            "Title,ArticleContent,PostDate,UpdateDate,ReplyCount,Lock,ArticleImage")] ArticleView article)
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

            var article = await _dbContext.ArticleViews
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
        public List<ArticleView>? ForumResult {
            get;
            set;
        }
    }
}
