using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Domain.Entities;

public class StockMovement
{
    public Guid Id { get; private set; }
    
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; }  = null!;
    
    public MovementType MovementType { get; private set; }
    public decimal Quantity { get; private set; }
    public string Reason { get; private set; }
    
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    
    public DateTime Date { get; private set; }

    public StockMovement(Guid productId, Guid userId, MovementType movementType, decimal quantity, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Name cannot be empty;");
        
        Id = Guid.NewGuid();
        ProductId = productId;
        MovementType = movementType;
        Quantity = quantity;
        Reason = reason;
        UserId = userId;
        Date = DateTime.UtcNow;
    }
}