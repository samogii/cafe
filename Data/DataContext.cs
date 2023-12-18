using Cafe.Models;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ReserveTable> ReserveTables { get; set; }
        public DbSet<ReservationTables> Reservations { get; set; }
    }
}
