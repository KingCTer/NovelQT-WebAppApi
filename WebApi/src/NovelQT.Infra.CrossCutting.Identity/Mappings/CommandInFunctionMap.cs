using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovelQT.Infra.CrossCutting.Identity.Models;

namespace NovelQT.Infra.CrossCutting.Identity.Mappings
{
    public class CommandInFunctionMap : IEntityTypeConfiguration<CommandInFunction>
    {
        public void Configure(EntityTypeBuilder<CommandInFunction> builder)
        {
            builder.ToTable("CommandInFunctions");

            builder.HasKey(x => new { x.CommandId, x.FunctionId });

            builder.Property(x => x.CommandId)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(x => x.FunctionId)
                .HasMaxLength(50)
                .IsUnicode(false);

        }
    }
}
