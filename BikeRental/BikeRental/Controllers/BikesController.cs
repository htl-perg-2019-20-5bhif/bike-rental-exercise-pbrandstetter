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
    public class BikesController : ControllerBase
    {
        private readonly BikeRentalContext _context;

        public BikesController(BikeRentalContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET all bikes: api/Bikes
        /// </summary>
        /// <returns>A list of bikes</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bike>>> GetBikes()
        {
            return await _context.Bikes.ToListAsync();
        }

        /// <summary>
        /// GET all available bikes: api/Bikes/available
        /// </summary>
        /// <param name="sortBy">Optional values: priceFirstHour (ascending), priceAdditionalHours (ascending), purchaseDate (descending)</param>
        /// <returns>A list of bikes</returns>
        [HttpGet("/available")]
        public async Task<ActionResult<IEnumerable<Bike>>> GetAvailableBikes([FromQuery]string sortBy = "")
        {
            var rentals = _context.Rentals;
            var bikes = _context.Bikes;
            var availableBikes = bikes.Where(b => rentals.Any(r => r.BikeId == b.Id));

            switch (sortBy)
            {
                case "": break;
                case "priceFirstHour": availableBikes.OrderBy(b => b.PriceFirstHour); break;
                case "priceAdditionalHours": availableBikes.OrderBy(b => b.PricePerAdditionalHour); break;
                case "purchaseDate": availableBikes.OrderByDescending(b => b.PurchaseDate); break;
                default: return NotFound("No such filter method found");
            }

            return await availableBikes.ToArrayAsync();
        }

        /// <summary>
        /// GET a specific bike: api/Bikes/5
        /// </summary>
        /// <param name="id">Unique id of bike</param>
        /// <returns>A bike with specified id</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Bike>> GetBike(int id)
        {
            var bike = await _context.Bikes.FindAsync(id);

            if (bike == null)
            {
                return NotFound();
            }

            return bike;
        }

        /// <summary>
        /// PUT a bike: api/Bikes/5
        /// </summary>
        /// <param name="id">Unique id of bike</param>
        /// <param name="bike">Bike with updated fields</param>
        /// <returns></returns>
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBike(int id, Bike bike)
        {
            if (id != bike.Id)
            {
                return BadRequest();
            }

            _context.Entry(bike).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BikeExists(id))
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
        /// POST a new Bike: api/Bikes
        /// </summary>
        /// <param name="bike">The bike that has to be added</param>
        /// <returns>The new bike with its Id</returns>
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Bike>> PostBike(Bike bike)
        {
            _context.Bikes.Add(bike);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBike", new { id = bike.Id }, bike);
        }

        /// <summary>
        /// DELETE a bike: api/Bikes/5
        /// </summary>
        /// <param name="id">Unique id of bike</param>
        /// <returns>The bike that was deleted</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Bike>> DeleteBike(int id)
        {
            var bike = await _context.Bikes.FindAsync(id);
            if (bike == null)
            {
                return NotFound();
            }

            _context.Bikes.Remove(bike);
            await _context.SaveChangesAsync();

            return bike;
        }

        /// <summary>
        /// Indicates if a bike already exists or not
        /// </summary>
        /// <param name="id">Unique id of bike</param>
        /// <returns>Boolean if customer exists or not</returns>
        private bool BikeExists(int id)
        {
            return _context.Bikes.Any(e => e.Id == id);
        }
    }
}
