using Kolokwium2.Models;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Data;


    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Concert> Concerts { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketConcert> TicketConcerts { get; set; }
        public DbSet<PurchasedTicket> PurchasedTickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PurchasedTicket>()
                .HasKey(pt => pt.TicketConcertId);

            modelBuilder.Entity<TicketConcert>()
                .HasOne(tc => tc.PurchasedTicket)
                .WithOne(pt => pt.TicketConcert)
                .HasForeignKey<PurchasedTicket>(pt => pt.TicketConcertId);

            modelBuilder.Entity<Customer>().HasData (new List<Customer>
            {
                new Customer() { CustomerId = 1, FirstName = "John" , LastName = "Bravo", PhoneNumber = "123456789" },
                new Customer() { CustomerId = 2, FirstName = "John" , LastName = "Doe", PhoneNumber = "234567891" }
            });

        }

    }
