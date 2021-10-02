using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovelQT.Infra.CrossCutting.Identity.Models;

namespace NovelQT.Infra.CrossCutting.Identity.Mappings
{
    public class PermissionMap : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");

            builder.HasKey(x => new { x.RoleId, x.FunctionId, x.CommandId });

            builder.Property(x => x.RoleId)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(x => x.FunctionId)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(x => x.CommandId)
                .HasMaxLength(50)
                .IsUnicode(false);
        }
    }
}
