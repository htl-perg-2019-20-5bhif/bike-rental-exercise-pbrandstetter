using BikeRentalService;
using BikeRentalService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRentalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly BikeRentalContext _context;

        public RentalsController(BikeRentalContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET all rentals: api/Rentals
        /// </summary>
        /// <returns>A list of rentals</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rental>>> GetRentals()
        {
            return await _context.Rentals.Include(r => r.Bike).Include(r => r.Customer).ToListAsync();
        }

        /// <summary>
        /// Get a list of unpaid, ended rentals with total price > 0: api/Rentals/unpaid
        /// </summary>
        /// <returns></returns>
        [HttpGet("/unpaid")]
        public async Task<ActionResult<IEnumerable<Rental>>> GetUnpaidRentals()
        {
            // TODO: Return: Customer's ID, first and last name, Rental's ID, start end, end date, and total price
            var rentals = _context.Rentals.Include(r => r.Bike).Include(r => r.Customer);
            return await rentals.Where(r => r.Paid == false && r.TotalCost > 0).ToListAsync();
        }

        /// <summary>
        /// GET a specific rental: api/Rentals/5
        /// </summary>
        /// <param name="id">Unique id of a rental</param>
        /// <returns>A rental with specified id</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Rental>> GetRental(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);

            if (rental == null)
            {
                return NotFound();
            }

            return rental;
        }

        /// <summary>
        /// PUT: End a rental and culculate costs: api/Rentals/5/end
        /// </summary>
        /// <param name="id">Unique id of a rental</param>
        /// <param name="rental">Rental with udpated fields</param>
        /// <returns></returns>
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}/end")]
        public async Task<IActionResult> EndRental(int id)
        {
            var rental = _context.Rentals.Where(r => r.Id == id).First();
            if (id != rental.Id)
            {
                return BadRequest();
            }

            _context.Entry(rental).State = EntityState.Modified;

            try
            {
                rental.RentalEnd = DateTime.Now;
                rental.TotalCost = CalculateTotalCosts(rental);
                rental.Paid = false;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentalExists(id))
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

        /// <summary>
        /// PUT: Mark an ended rental as paid: api/Rentals/5/pay
        /// </summary>
        /// <param name="id">Unique id of a rental</param>
        /// <returns></returns>
        [HttpPut("{id}/pay")]
        public async Task<IActionResult> PayRental(int id)
        {
            var rental = _context.Rentals.Where(r => r.Id == id).First();
            if (id != rental.Id)
            {
                return BadRequest();
            }

            _context.Entry(rental).State = EntityState.Modified;

            try
            {
                if (rental.TotalCost > 0)
                {
                    rental.Paid = true;
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentalExists(id))
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

        /// <summary>
        /// POST: Start a new rental: api/Rentals/start
        /// </summary>
        /// <param name="rental">The rental that has to be started</param>
        /// <returns>The new rental with its Id and RentalBegin</returns>
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("/start")]
        public async Task<ActionResult<Rental>> StartRental(Rental rental)
        {
            rental.RentalBegin = DateTime.Now;
            rental.RentalEnd = new DateTime();
            rental.TotalCost = 0;
            rental.Paid = false;
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRental", new { id = rental.Id }, rental);
        }

        /// <summary>
        /// DELETE a rental: api/Rentals/5
        /// </summary>
        /// <param name="id">Unique id of a rental</param>
        /// <returns>The rental that was deleted</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Rental>> DeleteRental(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
            {
                return NotFound();
            }

            _context.Rentals.Remove(rental);
            await _context.SaveChangesAsync();

            return rental;
        }

        /// <summary>
        /// Indicates if the rental already exists or not
        /// </summary>
        /// <param name="id">Unique id of a rental</param>
        /// <returns>Boolean if rental exists or not</returns>
        private bool RentalExists(int id)
        {
            return _context.Rentals.Any(e => e.Id == id);
        }

        /// <summary>
        /// Calculates the costs of a rental based on the bikes prices and the duration
        /// </summary>
        /// <param name="rental">Rental of which the costs should be calculated</param>
        /// <returns>Total costs of a rental</returns>
        private double CalculateTotalCosts(Rental rental)
        {
            var duration = rental.RentalEnd - rental.RentalBegin;
            double totalCost = 0;
            if (duration.Minutes <= 15)
            {
                return totalCost;
            }
            totalCost += rental.Bike.PriceFirstHour;
            if (duration.Hours - 1 > 0)
            {
                totalCost += (int)(Math.Ceiling(duration.TotalHours - 1)) * rental.Bike.PricePerAdditionalHour;
            }
            return totalCost;
        }
    }
}
