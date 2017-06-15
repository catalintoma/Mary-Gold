using System;
using Microsoft.EntityFrameworkCore;

namespace Marigold.Data
{
    public static class DbInitializer
    {
        public static void Initialize<TDbContext>(IUnitOfWork<TDbContext> uow) where TDbContext : DbContext
        {
            uow.DbContext.Database.EnsureCreated();
        }
    }
}