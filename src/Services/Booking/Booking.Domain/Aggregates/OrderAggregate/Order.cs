using Booking.Domain.SeedWork;

namespace Booking.Domain.Aggregates.OrderAggregate;

public class Order : Entity, IAggregateRoot
{
    // DDD: Los sets son privados. Solo se modifican vía métodos.
    public string? UserId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public OrderStatus Status { get; private set; }

    // Backing field para la colección
    private readonly List<OrderItem> _orderItems;
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

    public decimal Total => _orderItems.Sum(o => o.Quantity * o.UnitPrice);

    protected Order()
    {
        _orderItems = [];
    }

    public Order(string userId) : this()
    {
        UserId = userId;
        Status = OrderStatus.Submitted;
        OrderDate = DateTime.UtcNow;
    }

    // LÓGICA DE NEGOCIO: Añadir entrada
    public void AddOrderItem(int eventId, string eventName, decimal unitPrice, int quantity)
    {
        // Regla de negocio: Máximo 4 tickets por evento en la misma orden
        var existingOrderForProduct = _orderItems.SingleOrDefault(o => o.EventId == eventId);

        if (existingOrderForProduct != null)
        {
            if (_orderItems.Sum(i => i.Quantity) + quantity > 4)
            {
                throw new Exception("Maximum 4 tickets per order allowed");
            }
            existingOrderForProduct.AddUnits(quantity);
        }
        else
        {
            if (quantity > 4) throw new Exception("Maximum 4 tickets per order allowed");
            _orderItems.Add(new OrderItem(eventId, eventName, unitPrice, quantity));
        }
    }

    public void SetPaidStatus()
    {
        if (Status == OrderStatus.StockConfirmed)
        {
            Status = OrderStatus.Paid;
            // Aquí podríamos disparar un Domain Event "OrderPaid"
        }
    }
}