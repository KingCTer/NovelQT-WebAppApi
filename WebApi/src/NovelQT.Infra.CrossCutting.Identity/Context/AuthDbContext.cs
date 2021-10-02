using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NovelQT.Infra.CrossCutting.Identity.Context.Extensions;
using NovelQT.Infra.CrossCutting.Identity.Mappings;
using NovelQT.Infra.CrossCutting.Identity.Models;

namespace NovelQT.Infra.CrossCutting.Identity.Context
{
    public class AuthDbContext : IdentityDbContext<AppUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        // DbSet
        public DbSet<Command> Commands { set; get; }
        public DbSet<Function> Functions { set; get; }
        public DbSet<CommandInFunction> CommandInFunctions { set; get; }
        public DbSet<Permission> Permissions { set; get; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuration for AspNetCore.Identity
            builder.ApplyConfiguration(new AppUserMap());
            builder.Entity<IdentityRole>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);

            // Configuration for Model
            builder.ApplyConfiguration(new CommandMap());
            builder.ApplyConfiguration(new FunctionMap());
            builder.ApplyConfiguration(new CommandInFunctionMap());
            builder.ApplyConfiguration(new PermissionMap());

            // Seed
            builder.Seed();
        }
    }
}
