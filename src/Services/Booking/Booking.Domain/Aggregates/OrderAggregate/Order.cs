using Booking.Domain.SeedWork;

namespace Booking.Domain.Aggregates.OrderAggregate;

public class Order : Entity, IAggregateRoot
{
    // 1. Definimos la Regla de Negocio como constante pública
    public const int MaxTicketsPerOrder = 4;

    // 2. Definimos el mensaje de error reutilizando la constante
    private const string MaxTicketsExceededMessage = "Maximum {0} tickets per order allowed";

    public string UserId { get; private set; } = string.Empty;
    public DateTime OrderDate { get; private set; }
    public OrderStatus Status { get; private set; }

    private readonly List<OrderItem> _orderItems;
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

    public decimal Total => _orderItems.Sum(o => o.Quantity * o.UnitPrice);

    protected Order() => _orderItems = [];

    public Order(string userId) : this()
    {
        UserId = userId;
        Status = OrderStatus.Submitted;
        OrderDate = DateTime.UtcNow;
    }

    public void AddOrderItem(int eventId, string eventName, decimal unitPrice, int quantity)
    {
        var existingOrderForProduct = _orderItems.SingleOrDefault(o => o.EventId == eventId);

        // Calculamos la cantidad total proyectada
        var currentQuantity = existingOrderForProduct?.Quantity ?? 0;
        var projectedTotal = _orderItems.Sum(i => i.Quantity) - currentQuantity + quantity + currentQuantity;

        // Corrección: La validación debe ser sobre el total de tickets de la orden, no solo si agrega > 4 de golpe.
        // Si ya tengo 3 y agrego 2, suma 5. Eso debe fallar.
        int totalTicketsInOrder = _orderItems.Sum(i => i.Quantity);

        // Si es un item nuevo, simplemente sumamos. Si ya existe, calculamos la diferencia.
        // Simplificación: Validamos el total final esperado.
        if (totalTicketsInOrder + quantity > MaxTicketsPerOrder)
        {
            // Usamos string.Format para insertar el número dinámicamente
            throw new Exception(string.Format(MaxTicketsExceededMessage, MaxTicketsPerOrder));
        }

        if (existingOrderForProduct != null)
        {
            existingOrderForProduct.AddUnits(quantity);
        }
        else
        {
            _orderItems.Add(new OrderItem(eventId, eventName, unitPrice, quantity));
        }
    }

    public void SetPaidStatus()
    {
        if (Status == OrderStatus.StockConfirmed)
        {
            Status = OrderStatus.Paid;
        }
    }
}