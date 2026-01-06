using Booking.Domain.SeedWork;

namespace Booking.Domain.Aggregates.OrderAggregate;

public class OrderItem : Entity
{
    // Propiedades privadas con setters privados (Encapsulamiento)
    public string EventName { get; private set; } = string.Empty;
    public int EventId { get; private set; } // Referencia lógica a Mongo
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    protected OrderItem() { } // Para EF Core

    public OrderItem(int eventId, string eventName, decimal unitPrice, int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Invalid quantity");
        if (unitPrice < 0) throw new ArgumentException("Invalid price");

        EventId = eventId;
        EventName = eventName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public void AddUnits(int units)
    {
        if (units < 0) throw new ArgumentException("Invalid units");
        Quantity += units;
    }
}