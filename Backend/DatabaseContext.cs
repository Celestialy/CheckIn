using CheckIn.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend
{
    public class DatabaseContext : DbContext
    {
        // These are properly importent
        public DatabaseContext() { }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        // Database tables
        public DbSet<Card> Cards { get; set; }
        public DbSet<CheckTime> CheckTimes { get; set; }
        public DbSet<Scanner> Scanners { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Logging> Loggings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Logging>().Property(e => e.Action).HasConversion<string>();
            modelBuilder.Entity<Logging>().Property(e => e.Level).HasConversion<string>();
            modelBuilder.Entity<Logging>().Property(e => e.Type).HasConversion<string>();
        }
    }
}
