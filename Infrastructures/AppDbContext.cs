using Application.Interfaces;
using Domain.Entities;
using Infrastructures.FluentAPIs;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Infrastructures
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        # region DBSet
        public DbSet<User> Users { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new UserConfiguration().Configure(modelBuilder.Entity<User>());
        }

    }
}
