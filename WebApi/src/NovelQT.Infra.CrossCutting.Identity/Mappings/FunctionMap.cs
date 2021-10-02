using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovelQT.Infra.CrossCutting.Identity.Models;

namespace NovelQT.Infra.CrossCutting.Identity.Mappings
{
    public class FunctionMap : IEntityTypeConfiguration<Function>
    {
        public void Configure(EntityTypeBuilder<Function> builder)
        {
            builder.ToTable("Functions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode();

            builder.Property(x => x.ParentId)
                .IsRequired(false)
                .HasMaxLength(50)
                .IsUnicode(false);

        }
    }
}
