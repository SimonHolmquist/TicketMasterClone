using Booking.Domain.Aggregates.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Infrastructure.EntityConfigurations;

public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems", "booking");
        builder.HasKey(o => o.Id);

        builder.Property(o => o.EventName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(o => o.UnitPrice)
            .HasColumnType("decimal(18,2)"); // Importante para dinero

        // Relación: Un OrderItem pertenece a una Order
        // Si borras la Order, se borran los items (Cascade Delete)
        builder.HasOne<Order>()
            .WithMany(o => o.OrderItems)
            .HasForeignKey("OrderId") // Clave foránea oculta (Shadow Property)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}