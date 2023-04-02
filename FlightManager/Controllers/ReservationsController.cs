using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlightManager.Data;
using FlightManager.Models;
using System.Net.Mail;
using System.Net;

namespace FlightManager.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly AppDbContext _context;

        public ReservationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
              return _context.Reservations != null ? 
                          View(await _context.Reservations.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Reservations'  is null.");
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.FlightId = new SelectList(_context.Flights.ToList(), "Id", "PlaneId");

            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int flightId,[Bind("Id,FirstName,SecondName,LastName,PersonalID,PhoneNumber,Nationality,TicketType,Email")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                var flight = await _context.Flights.FindAsync(flightId);

                //Check if there are available seats for flights with regular tickets
                if (flight != null && reservation.TicketType == "Regular")
                {
                    if (flight.AvailableSeats > 0)
                    {
                        flight.AvailableSeats--;
                        _context.Entry(flight).State = EntityState.Modified;

                        reservation.FlightId = flightId;
                        _context.Add(reservation);

                        flight.Reservations.Add(reservation);

                        await _context.SaveChangesAsync();

                        //Sending email functionality gmail port 465/587
                        //Timeout
                        /*string recipientEmail = reservation.Email;
                        string subject = "Reservation successfully made";
                        string body = "Dear " + reservation.FirstName + ",\n\nYour reservation has been successfully made.";
                        string senderEmail = "flightmanager33@gmail.com";
                        string senderPassword = "nocturne.7n";
                        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                        smtpClient.EnableSsl = true;
                        smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                        MailMessage message = new MailMessage(senderEmail, recipientEmail, subject, body);
                        message.IsBodyHtml = false;
                        smtpClient.Send(message);*/

                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "No available seats for this flight.");
                        return View(reservation);
                    }
                }
                //Check if there are available seats for flights with business class tickets
                else if (flight != null && reservation.TicketType == "Business")
                {
                    if (flight.AvailableSeatsBusinessClass > 0)
                    {
                        flight.AvailableSeatsBusinessClass--;
                        _context.Entry(flight).State = EntityState.Modified;

                        reservation.FlightId = flightId;
                        _context.Add(reservation);
                        
                        flight.Reservations.Add(reservation) ;

                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "No available seats for this flight.");
                        return View(reservation);
                    }
                }
            }
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,SecondName,LastName,PersonalID,PhoneNumber,Nationality,TicketType,Email")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
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
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservations == null)
            {
                return Problem("Entity set 'AppDbContext.Reservations'  is null.");
            }
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
          return (_context.Reservations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
