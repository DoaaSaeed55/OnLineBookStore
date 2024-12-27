using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnLineBookStore.Models;

namespace OnLineBookStore.Data
{
    public class OnLineBookStoreContext : IdentityDbContext<AppUser>
    {
        public OnLineBookStoreContext (DbContextOptions<OnLineBookStoreContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; } = default!;
        public DbSet<CartItem> CartItems { get; set; } 
        public DbSet<Order> Orders { get; set; } 
        public DbSet<OrderItem> OrderItems { get; set; } 
    }
}
