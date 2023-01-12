using CardsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CardsAPI.Data
{
    public class CardsDbContext : DbContext
    {
        //Constructor
        public CardsDbContext(DbContextOptions options) : base(options)
        {
        }

        //DbSet - Tablica Cards u SQLu 
        public DbSet<Card> Cards { get; set; }

    }
}
