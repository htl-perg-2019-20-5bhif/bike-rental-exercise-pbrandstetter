using BikeRentalService.Model;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalService
{
    public class BikeRentalContext : DbContext
    {
        // TODO: Create Swagger document (Swashbuckle -> NUGet)
        // Document + Format code
        // "Design" rest api
        // Write automated tests

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        public BikeRentalContext(DbContextOptions<BikeRentalContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
