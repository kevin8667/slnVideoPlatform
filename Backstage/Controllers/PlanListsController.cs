using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backstage.Models;

namespace Backstage.Controllers
{
    //public class PlanListsController : Controller
    //{
    //    private readonly VideoDBContext _context;

    //    public PlanListsController(VideoDBContext context)
    //    {
    //        _context = context;
    //    }

    //    // GET: PlanLists
    //    public async Task<IActionResult> Index()
    //    {
    //        return View(await _context.PlanLists.ToListAsync());
    //    }

    //    // GET: PlanLists/Details/5
    //    public async Task<IActionResult> Details(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var planList = await _context.PlanLists
    //            .FirstOrDefaultAsync(m => m.PlanId == id);
    //        if (planList == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(planList);
    //    }

    //    // GET: PlanLists/Create
    //    public IActionResult Create()
    //    {
    //        return View();
    //    }

    //    // POST: PlanLists/Create
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create([Bind("PlanId,PlanName")] PlanList planList)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            _context.Add(planList);
    //            await _context.SaveChangesAsync();
    //            return RedirectToAction(nameof(Index));
    //        }
    //        return View(planList);
    //    }

    //    // GET: PlanLists/Edit/5
    //    public async Task<IActionResult> Edit(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var planList = await _context.PlanLists.FindAsync(id);
    //        if (planList == null)
    //        {
    //            return NotFound();
    //        }
    //        return View(planList);
    //    }

    //    // POST: PlanLists/Edit/5
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit(int id, [Bind("PlanId,PlanName")] PlanList planList)
    //    {
    //        if (id != planList.PlanId)
    //        {
    //            return NotFound();
    //        }

    //        if (ModelState.IsValid)
    //        {
    //            try
    //            {
    //                _context.Update(planList);
    //                await _context.SaveChangesAsync();
    //            }
    //            catch (DbUpdateConcurrencyException)
    //            {
    //                if (!PlanListExists(planList.PlanId))
    //                {
    //                    return NotFound();
    //                }
    //                else
    //                {
    //                    throw;
    //                }
    //            }
    //            return RedirectToAction(nameof(Index));
    //        }
    //        return View(planList);
    //    }

    //    // GET: PlanLists/Delete/5
    //    public async Task<IActionResult> Delete(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var planList = await _context.PlanLists
    //            .FirstOrDefaultAsync(m => m.PlanId == id);
    //        if (planList == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(planList);
    //    }

    //    // POST: PlanLists/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(int id)
    //    {
    //        var planList = await _context.PlanLists.FindAsync(id);
    //        if (planList != null)
    //        {
    //            _context.PlanLists.Remove(planList);
    //        }

    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }

    //    private bool PlanListExists(int id)
    //    {
    //        return _context.PlanLists.Any(e => e.PlanId == id);
    //    }
    //}
}
