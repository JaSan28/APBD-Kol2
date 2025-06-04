namespace Kolokwium2.DTOs;

public class CustomerWithTicketsDto
{
    public CustomerDto Customer { get; set; }
    public List<PurchaseDto> Purchases { get; set; }
}