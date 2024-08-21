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
    public class OrdersController : Controller
    {
        private readonly VideoDBContext _context;

        public OrdersController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var videoDBContext = _context.Orders.Include(o => o.Coupon).Include(o => o.Driver).Include(o => o.ShoppingCart);
            return View(await videoDBContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Coupon)
                .Include(o => o.Driver)
                .Include(o => o.ShoppingCart)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CouponId"] = new SelectList(_context.CouponInfos, "CouponId", "CouponName");
            ViewData["DriverId"] = new SelectList(_context.MemberInfos, "MemberId", "Email");
            ViewData["ShoppingCartId"] = new SelectList(_context.ShoppingCarts, "ShoppingCartId", "ShoppingCartId");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,ShoppingCartId,CouponId,OrderDate,OrderTotalPrice,DeliveryName,DeliveryAddress,PaymentStatus,DriverId,DeliveryStatus,LastEditTime")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CouponId"] = new SelectList(_context.CouponInfos, "CouponId", "CouponName", order.CouponId);
            ViewData["DriverId"] = new SelectList(_context.MemberInfos, "MemberId", "Email", order.DriverId);
            ViewData["ShoppingCartId"] = new SelectList(_context.ShoppingCarts, "ShoppingCartId", "ShoppingCartId", order.ShoppingCartId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CouponId"] = new SelectList(_context.CouponInfos, "CouponId", "CouponName", order.CouponId);
            ViewData["DriverId"] = new SelectList(_context.MemberInfos, "MemberId", "Email", order.DriverId);
            ViewData["ShoppingCartId"] = new SelectList(_context.ShoppingCarts, "ShoppingCartId", "ShoppingCartId", order.ShoppingCartId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,ShoppingCartId,CouponId,OrderDate,OrderTotalPrice,DeliveryName,DeliveryAddress,PaymentStatus,DriverId,DeliveryStatus,LastEditTime")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CouponId"] = new SelectList(_context.CouponInfos, "CouponId", "CouponName", order.CouponId);
            ViewData["DriverId"] = new SelectList(_context.MemberInfos, "MemberId", "Email", order.DriverId);
            ViewData["ShoppingCartId"] = new SelectList(_context.ShoppingCarts, "ShoppingCartId", "ShoppingCartId", order.ShoppingCartId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Coupon)
                .Include(o => o.Driver)
                .Include(o => o.ShoppingCart)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
