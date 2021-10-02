using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovelQT.Domain.Models;

namespace NovelQT.Infra.Data.Mappings
{
    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.Property(p => p.Name)
                .HasMaxLength(100)
                .IsUnicode()
                .IsRequired();

            builder.Property(c => c.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired();

            // builder.HasQueryFilter(m => EF.Property<bool>(m, "IsDeleted") == false);
            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}
