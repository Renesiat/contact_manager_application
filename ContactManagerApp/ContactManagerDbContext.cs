using Microsoft.EntityFrameworkCore;

using ContactManagerApp.Models;

namespace ContactManagerApp
{
    public class ContactManagerDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ContactManagerDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<Contact> Contacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>().HasData(
                new Contact {
                    Id = -1,
                    Name = "Someone",
                    DateOfBbirth = new DateTime(1990, 5, 15),
                    Married = true,
                    Phone = "987654321",
                    Salary = 456.78m

                });
        }
    }
}
