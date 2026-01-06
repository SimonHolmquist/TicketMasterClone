using Booking.Domain.SeedWork;

namespace Booking.Domain.Aggregates.OrderAggregate;

public class OrderItem : Entity
{
    // Constantes para mensajes de validación
    private const string InvalidQuantityMessage = "Invalid quantity";
    private const string InvalidPriceMessage = "Invalid price";
    private const string InvalidUnitsMessage = "Invalid units";

    public string EventName { get; private set; } = string.Empty;
    public int EventId { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    protected OrderItem() { }

    public OrderItem(int eventId, string eventName, decimal unitPrice, int quantity)
    {
        if (quantity <= 0) throw new ArgumentException(InvalidQuantityMessage);
        if (unitPrice < 0) throw new ArgumentException(InvalidPriceMessage);

        EventId = eventId;
        EventName = eventName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public void AddUnits(int units)
    {
        if (units < 0) throw new ArgumentException(InvalidUnitsMessage);
        Quantity += units;
    }
}