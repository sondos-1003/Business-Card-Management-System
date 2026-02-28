using Microsoft.EntityFrameworkCore;
using NeoRxTask.Entities;


namespace NeoRxTask.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {



        }



        public DbSet<BusinessCard> BusinessCard { get; set; }

    }
}
