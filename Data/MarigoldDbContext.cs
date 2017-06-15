using Microsoft.EntityFrameworkCore;

namespace Marigold.Data
{
    public class MarigoldDbContext : DbContext
    {
        public MarigoldDbContext(DbContextOptions<MarigoldDbContext> options) : base(options)
        {
            
        }
    }
}