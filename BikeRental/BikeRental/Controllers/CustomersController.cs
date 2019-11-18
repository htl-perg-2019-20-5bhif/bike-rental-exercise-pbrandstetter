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
    public class CustomersController : ControllerBase
    {
        private readonly BikeRentalContext _context;

        public CustomersController(BikeRentalContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET all customers: api/Customers
        /// OR filter by last name: api/Customers?lastName=Bar
        /// </summary>
        /// <param name="lastName">Optional filter parameter</param>
        /// <returns>A list of customers, optionally filtered by last name</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers([FromQuery]string lastName = "")
        {
            if (lastName == "")
            {
                return await _context.Customers.ToListAsync();
            }
            var customers = _context.Customers;
            return await customers.Where(c => c.LastName == lastName).ToListAsync();
        }

        /// <summary>
        /// GET a specific customer: api/Customers/5
        /// </summary>
        /// <param name="id">Unique id of customer</param>
        /// <returns>A customer with specified id</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        /// <summary>
        /// Get all rentals for a specific customer
        /// </summary>
        /// <param name="id">Unique id of customer</param>
        /// <returns>A list of rentals for the specified customer</returns>
        [HttpGet("{id}/rentals")]
        public async Task<ActionResult<IEnumerable<Rental>>> GetRentals(int id)
        {
            var rentals = _context.Rentals;
            return await rentals.Where(r => r.CustomerId == id).ToListAsync();
        }

        /// <summary>
        /// PUT a customer: api/Customers/5
        /// </summary>
        /// <param name="id">Unique id of customer</param>
        /// <param name="customer">Customer with udpated fields</param>
        /// <returns></returns>
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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
        /// POST a new Customer: api/Customers
        /// </summary>
        /// <param name="customer">The customer who has to be added</param>
        /// <returns>The new customer with her Id</returns>
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        /// <summary>
        /// DELETE a customer: api/Customers/5
        /// </summary>
        /// <param name="id">Unique id of customer</param>
        /// <returns>The customer who was deleted</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return customer;
        }

        /// <summary>
        /// Indicates if customer already exists or not
        /// </summary>
        /// <param name="id">Unique id of customer</param>
        /// <returns>Boolean if customer exists or not</returns>
        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
