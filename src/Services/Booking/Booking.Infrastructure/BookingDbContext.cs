using Booking.Domain.Aggregates.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Booking.Infrastructure;

public class BookingDbContext : DbContext
{
    // Constructor para pasar opciones (Connection String)
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica automáticamente todas las configuraciones que creamos en la carpeta EntityConfigurations
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}