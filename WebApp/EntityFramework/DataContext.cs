using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.EntityFramework
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<JobOffer> JobOffers { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().HasData(
                new Company { Id = 1, Name = "Microsoft" },
                new Company { Id = 2, Name = "Google" },
                new Company { Id = 3, Name = "Facebook" },
                new Company { Id = 4, Name = "Apple" },
                new Company { Id = 5, Name = "Nvidia" },
                new Company { Id = 6, Name = "Samsung" }
            );
        }
    }
}
