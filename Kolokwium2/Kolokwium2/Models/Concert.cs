using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Models;
[Table("Concert")]
[PrimaryKey(nameof(ConcertId))]

public class Concert
{
    [Key]
    public int ConcertId { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public int AvaliableTickets { get; set; }
}