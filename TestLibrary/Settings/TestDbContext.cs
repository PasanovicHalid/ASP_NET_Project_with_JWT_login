using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestLibrary.Users.Model;

namespace TestLibrary.Settings
{
    public sealed class TestDbContext : IdentityDbContext<User, UserRole, Guid>
    {
        public DbSet<Class1> Classes { get; set; }
        public TestDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
