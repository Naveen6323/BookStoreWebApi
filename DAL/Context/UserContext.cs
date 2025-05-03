using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context
{
    public class UserContext: DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }
        public DbSet<UserRegistrationModel> Users { get; set; }
        public DbSet<AdminRegistrationModel> Admins { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Cart> Carts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserRegistrationModel>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<AdminRegistrationModel>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
    
}
