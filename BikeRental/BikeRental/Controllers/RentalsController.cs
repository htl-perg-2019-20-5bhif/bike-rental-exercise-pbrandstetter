﻿using BikeRentalApi.Dtos;
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
        public ActionResult<IEnumerable<RentalDto>> GetRentals()
        {
            return RentalDto.ToRentalDto(_context.Rentals.Include(r => r.Bike).Include(r => r.Customer).ToList());
        }

        /// <summary>
        /// Get a list of unpaid, ended rentals with total price > 0: api/Rentals/unpaid
        /// </summary>
        /// <returns></returns>
        [HttpGet("unpaid")]
        public List<UnpaidRentalDto> GetUnpaidRentals()
        {
            var rentals = _context.Rentals
                .Include(r => r.Bike)
                .Include(r => r.Customer)
                .Where(r => r.Paid == false && r.TotalCost > 0);

            return UnpaidRentalDto.ToUnpaidRentalDto(rentals.ToList());
        }

        /// <summary>
        /// GET a specific rental: api/Rentals/5
        /// </summary>
        /// <param name="id">Unique id of a rental</param>
        /// <returns>A rental with specified id</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<RentalDto>> GetRental(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);

            if (rental == null)
            {
                return NotFound();
            }

            return RentalDto.ToRentalDto(rental);
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
                CostCalculator costCalculator = new CostCalculator();
                rental.RentalEnd = DateTime.Now;
                rental.TotalCost = costCalculator.CalculateTotalCosts(rental);
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
            // TODO: make this working with validator
            if (id != rental.Id || (rental.Paid && rental.TotalCost == 0) || (rental.RentalEnd == DateTime.MinValue && rental.Paid))
            {
                return BadRequest();
            }

            _context.Entry(rental).State = EntityState.Modified;

            try
            {
                rental.Paid = true;
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
        /// POST: Start a new rental: api/Rentals/start
        /// </summary>
        /// <param name="rental">The rental that has to be started</param>
        /// <returns>The new rental with its Id and RentalBegin</returns>
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("start")]
        public async Task<ActionResult<Rental>> StartRental(RentalDto rental)
        {
            Rental r = RentalDto.FromRentalDto(rental);
            var rentals = _context.Rentals;
            bool hasRental = rentals
                .Any(r => r.CustomerId == rental.CustomerId && (r.RentalEnd == DateTime.MinValue || !r.Paid));

            if (hasRental)
            {
                return BadRequest("The customer has a active rental");
            }

            r.RentalBegin = DateTime.Now;
            r.RentalEnd = new DateTime();
            r.TotalCost = 0;
            r.Paid = false;
            _context.Rentals.Add(r);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRental", new { id = rental.Id }, RentalDto.ToRentalDto(r));
        }

        /// <summary>
        /// DELETE a rental: api/Rentals/5
        /// </summary>
        /// <param name="id">Unique id of a rental</param>
        /// <returns>The rental that was deleted</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<RentalDto>> DeleteRental(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
            {
                return NotFound();
            }

            _context.Rentals.Remove(rental);
            await _context.SaveChangesAsync();

            return RentalDto.ToRentalDto(rental);
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
