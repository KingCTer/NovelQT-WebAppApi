using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NovelQT.Domain.Common.Constants;
using NovelQT.Infra.CrossCutting.Identity.Models;

namespace NovelQT.Infra.CrossCutting.Identity.Context.Extensions
{
    public static class SeedExtension
    {
        public static void Seed(this ModelBuilder builder)
        {
            // Seed Command
            builder.Entity<Command>().HasData(
                new Command() { Id = CommandCode.CREATE.ToString(), Name = "Thêm" },
                new Command() { Id = CommandCode.READ.ToString(), Name = "Xem" },
                new Command() { Id = CommandCode.UPDATE.ToString(), Name = "Sửa" },
                new Command() { Id = CommandCode.DELETE.ToString(), Name = "Xoá" },
                new Command() { Id = CommandCode.APPROVE.ToString(), Name = "Duyệt" }
            );

            //Seed Function
            builder.Entity<Function>().HasData(
                new Function { Id = FunctionCode.SYSTEM.ToString(), Name = "Hệ thống", SortOrder = 1, ParentId = null }
            );

            //Seed CommandInFunction
            builder.Entity<CommandInFunction>().HasData(
                new CommandInFunction() { FunctionId = FunctionCode.SYSTEM.ToString(), CommandId = CommandCode.CREATE.ToString() },
                new CommandInFunction() { FunctionId = FunctionCode.SYSTEM.ToString(), CommandId = CommandCode.READ.ToString() },
                new CommandInFunction() { FunctionId = FunctionCode.SYSTEM.ToString(), CommandId = CommandCode.UPDATE.ToString() },
                new CommandInFunction() { FunctionId = FunctionCode.SYSTEM.ToString(), CommandId = CommandCode.DELETE.ToString() },
                new CommandInFunction() { FunctionId = FunctionCode.SYSTEM.ToString(), CommandId = CommandCode.APPROVE.ToString() }
            );

            // Seed IdentityRole
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = IdentityConstant.Roles.AdminId,
                    Name = IdentityConstant.Roles.AdminId,
                    NormalizedName = IdentityConstant.Roles.AdminId.ToUpper()
                },
                new IdentityRole
                {
                    Id = IdentityConstant.Roles.UserId,
                    Name = IdentityConstant.Roles.UserId,
                    NormalizedName = IdentityConstant.Roles.UserId.ToUpper()
                }
            );

            // Seed Permission
            builder.Entity<Permission>().HasData(
                // Admin Permission
                new Permission() { RoleId = IdentityConstant.Roles.AdminId, FunctionId = FunctionCode.SYSTEM.ToString(), CommandId = CommandCode.CREATE.ToString() },
                new Permission() { RoleId = IdentityConstant.Roles.AdminId, FunctionId = FunctionCode.SYSTEM.ToString(), CommandId = CommandCode.READ.ToString() },
                new Permission() { RoleId = IdentityConstant.Roles.AdminId, FunctionId = FunctionCode.SYSTEM.ToString(), CommandId = CommandCode.UPDATE.ToString() },
                new Permission() { RoleId = IdentityConstant.Roles.AdminId, FunctionId = FunctionCode.SYSTEM.ToString(), CommandId = CommandCode.DELETE.ToString() },

                // User Permission
                new Permission() { RoleId = IdentityConstant.Roles.UserId, FunctionId = FunctionCode.SYSTEM.ToString(), CommandId = CommandCode.READ.ToString() }

            );
        }
    }
}
