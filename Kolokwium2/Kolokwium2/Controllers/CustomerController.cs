using Kolokwium2.Data;
using Kolokwium2.DTOs;
using Kolokwium2.Exceptions;
using Kolokwium2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly AppDbContext _context;
    
    public CustomersController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet("{customerId}/purchases")]
    public async Task<IActionResult> GetCustomerPurchases(int customerId)
    {
        var customer = await _context.Customers
            .Include(c => c.PurchasedTicket)
                .ThenInclude(pt => pt.TicketConcert)
                    .ThenInclude(tc => tc.Ticket)
            .Include(c => c.PurchasedTicket)
                .ThenInclude(pt => pt.TicketConcert)
                    .ThenInclude(tc => tc.Concert)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);
            
        if (customer == null)
        {
            return NotFound();
        }
        
        var result = new
        {
            firstName = customer.FirstName,
            lastName = customer.LastName,
            phoneNumber = customer.PhoneNumber,
            // purchases = customer.PurchasedTicket.Select(pt => new
            // {
            //     date = pt.PurchaseDate,
            //     price = pt.TicketConcert.Price,
            //     ticket = new
            //     {
            //         serial = pt.TicketConcert.Ticket.SerialNumber,
            //         seatNumber = pt.TicketConcert.Ticket.SeatNumber
            //     },
            //     concert = new
            //     {
            //         name = pt.TicketConcert.Concert.Name,
            //         date = pt.TicketConcert.Concert.Date
            //     }
            // })
        };
        
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddCustomerWithTickets([FromBody] CustomerWithTicketsDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var existingCustomer = await _context.Customers.FindAsync(dto.Customer.Id);
        if (existingCustomer != null)
        {
            return BadRequest("Customer already exists");
        }
        
        var customer = new Customer
        {
            CustomerId = dto.Customer.Id,
            FirstName = dto.Customer.FirstName,
            LastName = dto.Customer.LastName,
            PhoneNumber = dto.Customer.PhoneNumber
        };
        
        foreach (var purchase in dto.Purchases)
        {
            var concert = await _context.Concerts
                .FirstOrDefaultAsync(c => c.Name == purchase.ConcertName);
                
            if (concert == null)
            {
                return BadRequest($"Concert '{purchase.ConcertName}' not found");
            }
            
            var ticketsCount = await _context.PurchasedTickets
                .Include(pt => pt.TicketConcert)
                .Where(pt => pt.CustomerId == customer.CustomerId && 
                            pt.TicketConcert.ConcertId == concert.ConcertId)
                .CountAsync();
                
            if (ticketsCount + 1 > 5)
            {
                return Conflict($"Cannot purchase more than 5 tickets for concert '{{concert.Name}}'");
            }
  
            var ticket = new Ticket
            {
                SerialNumber = GenerateSerialNumber(),
                SeatNumber = purchase.SeatNumber
            };
      
            var ticketConcert = new TicketConcert
            {
                Ticket = ticket,
                Concert = concert,
                Price = purchase.Price
            };
            
            var purchasedTicket = new PurchasedTicket
            {
                Customer = customer,
                TicketConcert = ticketConcert,
                PurchaseDate = DateTime.Now
            };
            
            _context.Tickets.Add(ticket);
            _context.TicketConcerts.Add(ticketConcert);
            _context.PurchasedTickets.Add(purchasedTicket);
        }
        
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetCustomerPurchases), new { customerId = customer.CustomerId }, null);
    }
    
    private string GenerateSerialNumber()
    {
        return $"TK{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    }
}