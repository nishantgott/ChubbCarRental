using ChubbCarRental.Model;
using Microsoft.EntityFrameworkCore;

namespace ChubbCarRental.Data
{
    public class CarRentalDbContext : DbContext
    {
        public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options) { }
        public DbSet<CarModel> Cars { get; set; }
    }
}
