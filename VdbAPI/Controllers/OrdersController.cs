using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VdbAPI.Models;
using VdbAPI.ShoppingCartDTO;

namespace VdbAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly VideoDBContext _context;

        public OrdersController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var result = from o in _context.Orders
                         join sc in _context.ShoppingCarts on o.ShoppingCartId equals sc.ShoppingCartId
                         join plan in _context.PlanLists on sc.PlanId equals plan.PlanId
                         join video in _context.VideoLists on sc.VideoId equals video.VideoId
                         join cp in _context.CouponInfos on o.CouponId equals cp.CouponId
                         join mem in _context.MemberInfos on sc.MemberId equals mem.MemberId
                         select new getOrderDTO
                         {
                             OrderId = o.OrderId,
                             ShoppingCartId = sc.ShoppingCartId,
                             MemberId = sc.MemberId,
                             PlanId = sc.PlanId,
                             PlanName = plan.PlanName,
                             VideoId = sc.VideoId,
                             VideoName = video.VideoName,
                             CouponId = cp.CouponId,
                             CouponName = cp.CouponName,
                             OrderDate = o.OrderDate,
                             OrderTotalPrice = o.OrderTotalPrice,
                             DeliveryName = o.DeliveryName,
                             DeliveryAddress = o.DeliveryAddress,
                             PaymentStatus = o.PaymentStatus,
                             DeliveryStatus = o.DeliveryStatus,
                         };
            return Ok(await result.ToListAsync());
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
