using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovelQT.Domain.Models;

namespace NovelQT.Infra.Data.Mappings
{
    public class BookMap : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(x => x.Intro)
                .HasColumnType("ntext")
                .IsUnicode();


            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}
