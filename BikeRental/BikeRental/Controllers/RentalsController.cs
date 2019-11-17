using BikeRentalService;
using BikeRentalService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // TODO: Get a list of unpaid, ended rentals with total price > 0. (Return: Customer's ID, first and last name, Rental's ID, start end, end date, and total price)

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
        /// PUT a rental: api/Rentals/5
        /// </summary>
        /// <param name="id">Unique id of a rental</param>
        /// <param name="rental">Rental with udpated fields</param>
        /// <returns></returns>
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRental(int id, Rental rental)
        {
            // TODO: Rename to end rental
            // TODO: Rental end has to be set automatically based on the system time, Total costs are calculated automatically, IsPaid = false
            // TODO: change doc based on the new functionality
            if (id != rental.Id)
            {
                return BadRequest();
            }

            _context.Entry(rental).State = EntityState.Modified;

            try
            {
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

        // TODO: Add PUT request to set rental as paid (Can only be executed on rentals that have a total price > 0)

        /// <summary>
        /// POST: api/Rentals
        /// </summary>
        /// <param name="rental">The rental that has to be added</param>
        /// <returns>The new rental with its Id</returns>
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Rental>> PostRental(Rental rental)
        {
            // TODO: rename to StartRental
            // TODO: Rental begin has to be set automatically based on the system time, Rental end is empty, Total costs are empty, Not paid
            // TODO: change doc based on the new functionality
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
    }
}
