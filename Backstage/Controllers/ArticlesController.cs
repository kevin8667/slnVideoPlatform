using Backstage.Models;
using Backstage.Models.DTO;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Backstage.Controllers {
    public class ArticlesController : Controller {
        private readonly VideoDBContext _dbContext;
        private readonly IWebHostEnvironment _web; 
        private static List<Theme>? _cachedThemes;
        public ArticlesController(VideoDBContext context,IWebHostEnvironment environment)
        {
            _dbContext = context;
            _web = environment;
            if(_cachedThemes == null) {
                _cachedThemes = _dbContext.Themes.ToList();
            }
        }

        //GET: Articles
        public async Task<IActionResult> Index()
        {
            ViewBag.Theme = _cachedThemes;

            var articles = await _dbContext.ArticleViews
                .OrderBy(a => a.PostDate) // 可以根據需要進行排序
                .Take(1) // 加載初始顯示的資料量
                .ToListAsync();

            return View(articles);
        }

        [HttpPost]
        public async Task<IActionResult> LoadIndex([FromBody] forumDto searchDTO)
        {
            try {
                IQueryable<ArticleView> article = searchDTO.categoryId == 0 ? _dbContext.ArticleViews :
                    _dbContext.ArticleViews.Where(c => c.ThemeId == searchDTO.categoryId);

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
                    article = searchDTO.sortType == "asc" ? article.OrderBy(s => s.MemberName) :
                                                                article.OrderByDescending(s => s.MemberName);
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
                int dataCount = await article.CountAsync();

                // 分頁
                int PagesSize = searchDTO.pageSize ?? 10;
                int Page = searchDTO.page ?? 1;
                int TotalPages = (int)Math.Ceiling(((decimal)dataCount / PagesSize));

                // 跳過指定頁數的資料並取出當前頁面的資料
                var articles = await article.Skip((Page - 1) * PagesSize).Take(PagesSize).ToListAsync();

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

        [HttpPost]
        public IActionResult Register(UserInfo user)
        {
            if(user == null || user.UserPhoto == null || user.UserPhoto.Length == 0)
                return BadRequest(new {
                    error = "圖片或資料未填入"
                });

            try {
                string rootPath = _web.WebRootPath;
                string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(user.UserPhoto.FileName);
                string filePath = $"/img/{newFileName}";

                using(var filestream = new FileStream(rootPath + filePath,FileMode.Create)) {
                    user.UserPhoto.CopyTo(filestream);
                }

                return Json(Url.Content("~"+filePath));
            }
            catch(Exception ex) {
                return NotFound(new {
                    error = ex.Message + "發生例外的狀況",
                });
            }
        }

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
            ViewData["AuthorId"] = new SelectList( _dbContext.MemberInfos,
                "MemberId","MemberName",article.AuthorId);
            ViewData["ThemeId"] = new SelectList(_cachedThemes,
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
            ViewData["ThemeId"] = new SelectList(_cachedThemes,
                "ThemeId","ThemeName",article.ThemeId);
            return View(article);
        }


        private bool ArticleExists(int id)
        {
            return _dbContext.Articles.Any(e => e.ArticleId == id);
        }
    }

    public class UserInfo {
        public IFormFile? UserPhoto {
            get;
            set;
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
