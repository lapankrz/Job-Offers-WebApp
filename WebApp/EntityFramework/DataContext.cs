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
    }
}
