using Microsoft.EntityFrameworkCore;
using AutorizationExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationExample
{
    public class ProjectActivityDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public ProjectActivityDbContext(DbContextOptions<ProjectActivityDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
        }
    }

}
