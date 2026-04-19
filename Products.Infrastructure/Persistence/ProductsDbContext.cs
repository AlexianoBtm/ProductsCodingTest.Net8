using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Products.Domain.Entities;

namespace Products.Infrastructure.Persistence;

public class ProductsDbContext : DbContext
{
    public ProductsDbContext(DbContextOptions<ProductsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var decimalConverter = new ValueConverter<decimal, double>(
            v => Convert.ToDouble(v),
            v => Convert.ToDecimal(v));

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(p => p.Description)
                .HasMaxLength(1000);

            entity.Property(p => p.Colour)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.Price)
                .HasConversion(decimalConverter)
                .IsRequired();

            entity.Property(p => p.CreatedAtUtc)
                .IsRequired();
        });
    }
}