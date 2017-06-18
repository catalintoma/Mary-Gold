using Microsoft.EntityFrameworkCore;

namespace Marigold
{
    public class MarigoldDbContext : DbContext
    {
        public MarigoldDbContext(DbContextOptions<MarigoldDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Service> Services{get;set;}

        public DbSet<Room> Rooms{get;set;}

        public DbSet<Unit> Units{get;set;}

        public DbSet<Reservation> Reservations{get;set;}

        public DbSet<BillableService> BillableServices{get;set;}
    }
}