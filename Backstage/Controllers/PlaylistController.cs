using Backstage.Models;
using Backstage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace Backstage.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly VideoDBContext _context;
        private readonly ICompositeViewEngine _viewEngine;

        public PlaylistController(VideoDBContext context, ICompositeViewEngine viewEngine)
        {
            _context = context;
            _viewEngine = viewEngine;
        }

        // GET: PlayList/Index
        public async Task<IActionResult> Index()
        {
            var viewModel = new PlaylistViewModel
            {
                PlayLists = await _context.PlayLists.ToListAsync(),
                PlayListCollaborators = await _context.PlayListCollaborators.ToListAsync(),
                PlayListItems = await _context.PlayListItems.ToListAsync(),
                MemberPlayLists = await _context.MemberPlayLists.ToListAsync()
            };
            return View(viewModel);
        }

        public IActionResult LoadPagedPlayLists(int page = 1, int pageSize = 5, string sortBy = "PlayListId", string sortOrder = "asc")
        {
            var query = _context.PlayLists.AsQueryable();

            switch (sortBy)
            {
                case "PlayListName":
                    query = sortOrder == "asc" ? query.OrderBy(p => p.PlayListName) : query.OrderByDescending(p => p.PlayListName);
                    break;
                case "PlayListCreatedAt":
                    query = sortOrder == "asc" ? query.OrderBy(p => p.PlayListCreatedAt) : query.OrderByDescending(p => p.PlayListCreatedAt);
                    break;
                case "PlayListId":
                default:
                    query = sortOrder == "asc" ? query.OrderBy(p => p.PlayListId) : query.OrderByDescending(p => p.PlayListId);
                    break;
            }
                        
            var totalItems = query.Count();
            var playlists = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                        
            var viewModel = new PlaylistViewModel
            {
                PlayLists = playlists,
                CurrentPage = page,
                TotalPages = totalPages,
                SortOrder = sortOrder,
                SortBy = sortBy,
                PageSize = pageSize
            };

            var html = RenderPartialViewToString("_PlayListPartial", viewModel.PlayLists).Result;

            return Json(new { html = html, totalPages = totalPages });
        }

        private async Task<string> RenderPartialViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);

                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"{viewName} does not match any available view");
                }

                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }

        [HttpGet]
        public PartialViewResult FilterPlayLists(string searchTerm)
        {
            var filteredPlaylists = _context.PlayLists
                                            .Where(p => p.PlayListDescription.Contains(searchTerm))
                                            .ToList();

            return PartialView("_PlayListPartial", filteredPlaylists);
        }

        // GET: PlayList/Details/5
        public async Task<IActionResult> PlayListDetails(int? id)
        {
            if (id == null) return NotFound();

            var playList = await _context.PlayLists
                .FirstOrDefaultAsync(m => m.PlayListId == id);
            if (playList == null) return NotFound();

            return PartialView("_PlayListDetailsPartial", playList);
        }

        private async Task SaveUploadedFileAsync(PlayList playList)
        {
            if (Request.Form.Files["ShowImage"] != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await Request.Form.Files["ShowImage"].CopyToAsync(memoryStream);
                    playList.ShowImage = memoryStream.ToArray();
                }
            }
        }

        // GET: PlayList/GetPicture/5
        [HttpGet]
        public async Task<FileResult> GetPicture(int id)
        {
            var playList = await _context.PlayLists.FindAsync(id);
            if (playList?.ShowImage != null)
            {
                return File(playList.ShowImage, "image/jpeg");
            }
            else
            {
                return File("~/img/noimageooo.jpg", "image/jpeg");
            }
        }

        // GET: PlayList/Create
        public IActionResult PlayListCreate()
        {
            return PartialView("_PlayListCreatePartial");
        }

        // POST: PlayList/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlayListCreate([Bind("PlayListId,PlayListName,PlayListDescription,ViewCount,LikeCount,AddedCount,SharedCount,PlayListImage,ShowImage,PlayListCreatedAt,PlayListUpdatedAt,AnalysisTimestamp")] PlayList playList)
        {
            if (ModelState.IsValid)
            {
                await SaveUploadedFileAsync(playList);
                _context.Add(playList);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return PartialView("_PlayListCreatePartial", playList);
        }

        // GET: PlayList/Edit/5
        public async Task<IActionResult> PlayListEdit(int? id)
        {
            if (id == null) return NotFound();

            var playList = await _context.PlayLists.FindAsync(id);
            if (playList == null) return NotFound();

            return PartialView("_PlayListEditPartial", playList);
        }

        // POST: PlayList/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlayListEdit(int id, [Bind("PlayListId,PlayListName,PlayListDescription,ViewCount,LikeCount,AddedCount,SharedCount,PlayListImage,ShowImage,PlayListCreatedAt,PlayListUpdatedAt,AnalysisTimestamp")] PlayList playList)
        {
            if (id != playList.PlayListId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingPlayList = await _context.PlayLists.FindAsync(playList.PlayListId);
                    if (existingPlayList == null) return NotFound();

                    if (Request.Form.Files["ShowImage"] != null)
                    {
                        await SaveUploadedFileAsync(playList);
                    }
                    else
                    {
                        playList.ShowImage = existingPlayList.ShowImage;
                    }

                    _context.Entry(existingPlayList).State = EntityState.Detached;
                    _context.Update(playList);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");

                    //return PartialView("_PlayListPartial", await _context.PlayLists.ToListAsync());
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayListExists(playList.PlayListId)) return NotFound();
                    else throw;
                }
            }
            return PartialView("_PlayListEditPartial", playList);
        }

        [HttpPost]
        public async Task<IActionResult> PlayListDeleteConfirmed(int id)
        {
            var playList = await _context.PlayLists.FindAsync(id);
            if (playList != null)
            {
                _context.PlayLists.Remove(playList);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {                    
                    return Json(new { success = false, message = ex.Message });
                }
            }

            return PartialView("_PlayListPartial", await _context.PlayLists.ToListAsync());
        }

        // Action to load the PlayList Partial View
        public async Task<IActionResult> LoadPlayListPartial()
        {
            var playlists = await _context.PlayLists.ToListAsync();
            return PartialView("_PlayListPartial", playlists);
        }

        private bool PlayListExists(int id)
        {
            return _context.PlayLists.Any(e => e.PlayListId == id);
        }

        // CRUD for PlayListItems
        public async Task<IActionResult> PlayListItemDetails(int? id)
        {
            if (id == null) return NotFound();

            var playListItem = await _context.PlayListItems
                .FirstOrDefaultAsync(m => m.PlayListItemId == id);
            if (playListItem == null) return NotFound();

            return PartialView("_PlayListItemDetailsPartial", playListItem);
        }

        public IActionResult PlayListItemCreate()
        {
            return PartialView("_PlayListItemCreatePartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlayListItemCreate([Bind("PlayListItemId,PlayListId,VideoId,VideoPosition,VideoAddedAt")] PlayListItem playListItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(playListItem);
                await _context.SaveChangesAsync();
                return PartialView("_PlayListItemPartial", await _context.PlayListItems.ToListAsync());
            }
            return PartialView("_PlayListItemCreatePartial", playListItem);
        }

        public async Task<IActionResult> PlayListItemEdit(int? id)
        {
            if (id == null) return NotFound();

            var playListItem = await _context.PlayListItems.FindAsync(id);
            if (playListItem == null) return NotFound();

            return PartialView("_PlayListItemEditPartial", playListItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlayListItemEdit(int id, [Bind("PlayListItemId,PlayListId,VideoId,VideoPosition,VideoAddedAt")] PlayListItem playListItem)
        {
            if (id != playListItem.PlayListItemId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playListItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayListItemExists(playListItem.PlayListItemId)) return NotFound();
                    else throw;
                }
                return PartialView("_PlayListItemPartial", await _context.PlayListItems.ToListAsync());
            }
            return PartialView("_PlayListItemEditPartial", playListItem);
        }

        public async Task<IActionResult> PlayListItemDelete(int? id)
        {
            if (id == null) return NotFound();

            var playListItem = await _context.PlayListItems
                .FirstOrDefaultAsync(m => m.PlayListItemId == id);
            if (playListItem == null) return NotFound();

            return PartialView("_PlayListItemDeletePartial", playListItem);
        }

        [HttpPost, ActionName("PlayListItemDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlayListItemDeleteConfirmed(int id)
        {
            var playListItem = await _context.PlayListItems.FindAsync(id);
            if (playListItem != null)
            {
                _context.PlayListItems.Remove(playListItem);
                await _context.SaveChangesAsync();
            }

            return PartialView("_PlayListItemPartial", await _context.PlayListItems.ToListAsync());
        }
        // Action to load the PlayListItem Partial View
        public async Task<IActionResult> LoadPlayListItemPartial()
        {
            var playListItems = await _context.PlayListItems.ToListAsync();
            return PartialView("_PlayListItemPartial", playListItems);
        }
        private bool PlayListItemExists(int id)
        {
            return _context.PlayListItems.Any(e => e.PlayListItemId == id);
        }

        // CRUD for MemberPlayLists
        public async Task<IActionResult> MemberPlayListDetails(int? id)
        {
            if (id == null) return NotFound();

            var memberPlayList = await _context.MemberPlayLists
                .FirstOrDefaultAsync(m => m.MemberPlayListId == id);
            if (memberPlayList == null) return NotFound();

            return PartialView("_MemberPlayListDetailsPartial", memberPlayList);
        }

        public IActionResult MemberPlayListCreate()
        {
            return PartialView("_MemberPlayListCreatePartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MemberPlayListCreate([Bind("MemberPlayListId,MemberId,PlayListId,AddedOtherMemberPlayListAt")] MemberPlayList memberPlayList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(memberPlayList);
                await _context.SaveChangesAsync();
                return PartialView("_MemberPlayListPartial", await _context.MemberPlayLists.ToListAsync());
            }
            return PartialView("_MemberPlayListCreatePartial", memberPlayList);
        }

        public async Task<IActionResult> MemberPlayListEdit(int? id)
        {
            if (id == null) return NotFound();

            var memberPlayList = await _context.MemberPlayLists.FindAsync(id);
            if (memberPlayList == null) return NotFound();

            return PartialView("_MemberPlayListEditPartial", memberPlayList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MemberPlayListEdit(int id, [Bind("MemberPlayListId,MemberId,PlayListId,AddedOtherMemberPlayListAt")] MemberPlayList memberPlayList)
        {
            if (id != memberPlayList.MemberPlayListId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(memberPlayList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberPlayListExists(memberPlayList.MemberPlayListId)) return NotFound();
                    else throw;
                }
                return PartialView("_MemberPlayListPartial", await _context.MemberPlayLists.ToListAsync());
            }
            return PartialView("_MemberPlayListEditPartial", memberPlayList);
        }

        public async Task<IActionResult> MemberPlayListDelete(int? id)
        {
            if (id == null) return NotFound();

            var memberPlayList = await _context.MemberPlayLists
                .FirstOrDefaultAsync(m => m.MemberPlayListId == id);
            if (memberPlayList == null) return NotFound();

            return PartialView("_MemberPlayListDeletePartial", memberPlayList);
        }

        [HttpPost, ActionName("MemberPlayListDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MemberPlayListDeleteConfirmed(int id)
        {
            var memberPlayList = await _context.MemberPlayLists.FindAsync(id);
            if (memberPlayList != null)
            {
                _context.MemberPlayLists.Remove(memberPlayList);
                await _context.SaveChangesAsync();
            }

            return PartialView("_MemberPlayListPartial", await _context.MemberPlayLists.ToListAsync());
        }

        // Action to load the MemberPlayList Partial View
        public async Task<IActionResult> LoadMemberPlayListPartial()
        {
            var memberPlayLists = await _context.MemberPlayLists.ToListAsync();
            return PartialView("_MemberPlayListPartial", memberPlayLists);
        }

        private bool MemberPlayListExists(int id)
        {
            return _context.MemberPlayLists.Any(e => e.MemberPlayListId == id);
        }

        // CRUD for PlayListCollaborators
        public async Task<IActionResult> PlayListCollaboratorDetails(int? id)
        {
            if (id == null) return NotFound();

            var playListCollaborator = await _context.PlayListCollaborators
                .FirstOrDefaultAsync(m => m.CollaboratorId == id);
            if (playListCollaborator == null) return NotFound();

            return PartialView("_PlayListCollaboratorDetailsPartial", playListCollaborator);
        }

        public IActionResult PlayListCollaboratorCreate()
        {
            return PartialView("_PlayListCollaboratorCreatePartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlayListCollaboratorCreate([Bind("CollaboratorId,PlayListId,MemberId,CollaboratorJoinedAt,CollaboratorActionType,ActionTimestamp")] PlayListCollaborator playListCollaborator)
        {
            if (ModelState.IsValid)
            {
                _context.Add(playListCollaborator);
                await _context.SaveChangesAsync();
                return PartialView("_PlayListCollaboratorPartial", await _context.PlayListCollaborators.ToListAsync());
            }
            return PartialView("_PlayListCollaboratorCreatePartial", playListCollaborator);
        }

        public async Task<IActionResult> PlayListCollaboratorEdit(int? id)
        {
            if (id == null) return NotFound();

            var playListCollaborator = await _context.PlayListCollaborators.FindAsync(id);
            if (playListCollaborator == null) return NotFound();

            return PartialView("_PlayListCollaboratorEditPartial", playListCollaborator);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlayListCollaboratorEdit(int id, [Bind("CollaboratorId,PlayListId,MemberId,CollaboratorJoinedAt,CollaboratorActionType,ActionTimestamp")] PlayListCollaborator playListCollaborator)
        {
            if (id != playListCollaborator.CollaboratorId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playListCollaborator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayListCollaboratorExists(playListCollaborator.CollaboratorId)) return NotFound();
                    else throw;
                }
                return PartialView("_PlayListCollaboratorPartial", await _context.PlayListCollaborators.ToListAsync());
            }
            return PartialView("_PlayListCollaboratorEditPartial", playListCollaborator);
        }

        public async Task<IActionResult> PlayListCollaboratorDelete(int? id)
        {
            if (id == null) return NotFound();

            var playListCollaborator = await _context.PlayListCollaborators
                .FirstOrDefaultAsync(m => m.CollaboratorId == id);
            if (playListCollaborator == null) return NotFound();

            return PartialView("_PlayListCollaboratorDeletePartial", playListCollaborator);
        }

        [HttpPost, ActionName("PlayListCollaboratorDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlayListCollaboratorDeleteConfirmed(int id)
        {
            var playListCollaborator = await _context.PlayListCollaborators.FindAsync(id);
            if (playListCollaborator != null)
            {
                _context.PlayListCollaborators.Remove(playListCollaborator);
                await _context.SaveChangesAsync();
            }

            return PartialView("_PlayListCollaboratorPartial", await _context.PlayListCollaborators.ToListAsync());
        }

        // Action to load the PlayListCollaborator Partial View
        public async Task<IActionResult> LoadPlayListCollaboratorPartial()
        {
            var playListCollaborators = await _context.PlayListCollaborators.ToListAsync();
            return PartialView("_PlayListCollaboratorPartial", playListCollaborators);
        }

        private bool PlayListCollaboratorExists(int id)
        {
            return _context.PlayListCollaborators.Any(e => e.CollaboratorId == id);
        }
    }
}
