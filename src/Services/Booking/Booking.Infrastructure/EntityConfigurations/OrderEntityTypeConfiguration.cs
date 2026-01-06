using Booking.Domain.Aggregates.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Infrastructure.EntityConfigurations;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // 1. Definir la tabla y la clave primaria
        builder.ToTable("Orders", "booking"); // Esquema 'booking' para organizar
        builder.HasKey(o => o.Id);

        // 2. Propiedades simples
        builder.Property(o => o.UserId)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(o => o.OrderDate)
            .IsRequired();

        // 3. Conversión de Enum a String (mejor legibilidad en SQL)
        builder.Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(30);

        // 4. Configurar la relación con los Items
        // Importante: Le decimos a EF que use el campo privado "_orderItems"
        var navigation = builder.Metadata.FindNavigation(nameof(Order.OrderItems));
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        // 5. Ignorar propiedades calculadas (no se guardan en BD)
        builder.Ignore(o => o.Total);
    }
}