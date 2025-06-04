using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Models;

[Table("Ticket")]
[PrimaryKey(nameof(TicketId))]
public class Ticket
{
    [Key]
    public int TicketId { get; set; }
    [MaxLength(50)]
    public string SerialNumber { get; set; }
    public int SeatNumber { get; set; }
}