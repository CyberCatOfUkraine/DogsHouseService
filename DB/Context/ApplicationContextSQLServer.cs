using DB.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Context
{
    internal class ApplicationContextSQLServer:DbContext
    {
        public DbSet<Dog> Dogs { get; set; }

        public ApplicationContextSQLServer()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dog>().HasKey(u => u.Id);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=dogsdb;Trusted_Connection=True;");
        }
    }
}
