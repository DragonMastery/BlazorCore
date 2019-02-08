using System;
using System.Collections.Generic;
using System.Text;
using BlazorCore.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorCore.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Contact>(b =>
            {
                b.Property(e => e.Status)
                    .HasConversion<string>();
            });

            // Customize the ASP.NET Identity model and override the defaults if needed. For example,
            // you can rename the ASP.NET Identity table names and more. Add your customizations
            // after calling base.OnModelCreating(builder);
        }
    }
}