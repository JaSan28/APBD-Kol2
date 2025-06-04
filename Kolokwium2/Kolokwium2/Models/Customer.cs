using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Models;

[Table("Customer")]
[PrimaryKey(nameof(CustomerId))]

public class Customer
{
    [Key]
    public int CustomerId { get; set; }
    [MaxLength(50)] 
    public string FirstName { get; set; }
    [MaxLength(100)]
    public string LastName { get; set; }
    [MaxLength(100)]
    public string PhoneNumber { get; set; }

    public virtual PurchasedTicket PurchasedTicket { get; set; }
}