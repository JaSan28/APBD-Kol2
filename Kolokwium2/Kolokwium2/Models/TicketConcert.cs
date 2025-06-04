using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Kolokwium2.Models;

public class TicketConcert
{
    [Key]
    public int TicketConcertId { get; set; }
    [ForeignKey(nameof(Ticket))]
    public int TicketId { get; set; }
    [ForeignKey(nameof(Concert))]
    public int ConcertId { get; set; }
    
    public decimal Price { get; set; }
    public virtual PurchasedTicket PurchasedTicket { get; set; }
    public virtual Ticket Ticket { get; set; }
    public virtual Concert Concert { get; set; }
}