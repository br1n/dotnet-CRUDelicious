using CRUDelicious.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDelicious
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options){}
        public DbSet<Dish> Dishes {get; set;}

    }

}