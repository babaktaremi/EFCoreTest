using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EFCoreTest.Model
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<User> Users => base.Set<User>();
        public DbSet<Book> Books => base.Set<Book>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Data Source=localhost;Initial Catalog=TestDB;Integrated Security=true");
                optionsBuilder.LogTo(Console.WriteLine);
            }
        }
    }
}
