
using EtkinlikYönetimProjesi.Model;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

using static System.Runtime.InteropServices.JavaScript.JSType;

using System;
using EtkinlikYönetimProjesi.Settings;

namespace Model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }
 
        public DbSet<User> Users { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMapping());
            base.OnModelCreating(modelBuilder);
        }
    }
}