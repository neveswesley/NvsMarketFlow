using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Domain.Entities;

public class StockMovement
{
    public Guid Id { get; set; }
    
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = new Product();
    
    public MovementType MovementType { get; set; }
    public decimal Quantity { get; set; }
    public string Reason { get; set; } = string.Empty;
    
    public Guid UserId { get; set; }
    public User User { get; set; } = new User();
    
    public DateTime Date { get; set; }
}