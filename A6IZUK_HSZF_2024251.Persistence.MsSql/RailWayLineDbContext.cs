using A6IZUK_HSZF_2024251.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace A6IZUK_HSZF_2024251.Persistence.MsSql
{
    public class RailWayLineDbContext : DbContext
    {
        public DbSet<RailwayLine> RailwaysLines { get; set; }
        public DbSet<Service> Services { get; set; }

        public RailWayLineDbContext(DbContextOptions<RailWayLineDbContext> options)
        : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RailwayLine>()
                .HasMany(rl => rl.Services)
                .WithOne(s => s.RailwayLine)
                .HasForeignKey(s => s.RailwayLineId);

            base.OnModelCreating(modelBuilder);
        }
    }

}
