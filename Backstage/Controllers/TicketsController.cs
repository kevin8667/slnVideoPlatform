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
    public class TicketsController : Controller
    {
        private readonly VideoDBContext _context;

        public TicketsController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var videoDBContext = _context.Tickets.Include(t => t.ReservationId).Include(t => t.Seat).Include(t => t.Showtime).Include(t => t.TypeOfTicketNavigation);
            return View(await videoDBContext.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.ReservationId)
                .Include(t => t.Seat)
                .Include(t => t.Showtime)
                .Include(t => t.TypeOfTicketNavigation)
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["ReservationId"] = new SelectList(_context.ReservationDetails, "ReservationId", "ReservationId");
            ViewData["SeatId"] = new SelectList(_context.Seats, "SeatId", "SeatId");
            ViewData["ShowtimeId"] = new SelectList(_context.Showtimes, "ShowtimeId", "ShowtimeId");
            ViewData["TypeOfTicket"] = new SelectList(_context.TypeOfTickets, "TypeOfTicket1", "TypeOfTicket1");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TicketId,ReservationId,TypeOfTicket,ShowtimeId,SeatId,Price")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ReservationId"] = new SelectList(_context.ReservationDetails, "ReservationId", "ReservationId", ticket.ReservationId);
            ViewData["SeatId"] = new SelectList(_context.Seats, "SeatId", "SeatId", ticket.SeatId);
            ViewData["ShowtimeId"] = new SelectList(_context.Showtimes, "ShowtimeId", "ShowtimeId", ticket.ShowtimeId);
            ViewData["TypeOfTicket"] = new SelectList(_context.TypeOfTickets, "TypeOfTicket1", "TypeOfTicket1", ticket.TypeOfTicket);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["ReservationId"] = new SelectList(_context.ReservationDetails, "ReservationId", "ReservationId", ticket.ReservationId);
            ViewData["SeatId"] = new SelectList(_context.Seats, "SeatId", "SeatId", ticket.SeatId);
            ViewData["ShowtimeId"] = new SelectList(_context.Showtimes, "ShowtimeId", "ShowtimeId", ticket.ShowtimeId);
            ViewData["TypeOfTicket"] = new SelectList(_context.TypeOfTickets, "TypeOfTicket1", "TypeOfTicket1", ticket.TypeOfTicket);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TicketId,ReservationId,TypeOfTicket,ShowtimeId,SeatId,Price")] Ticket ticket)
        {
            if (id != ticket.TicketId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.TicketId))
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
            ViewData["ReservationId"] = new SelectList(_context.ReservationDetails, "ReservationId", "ReservationId", ticket.ReservationId);
            ViewData["SeatId"] = new SelectList(_context.Seats, "SeatId", "SeatId", ticket.SeatId);
            ViewData["ShowtimeId"] = new SelectList(_context.Showtimes, "ShowtimeId", "ShowtimeId", ticket.ShowtimeId);
            ViewData["TypeOfTicket"] = new SelectList(_context.TypeOfTickets, "TypeOfTicket1", "TypeOfTicket1", ticket.TypeOfTicket);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.ReservationId)
                .Include(t => t.Seat)
                .Include(t => t.Showtime)
                .Include(t => t.TypeOfTicketNavigation)
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.TicketId == id);
        }
    }
}
