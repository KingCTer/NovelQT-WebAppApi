using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovelQT.Infra.Data.Migrations
{
    public partial class AddIndexStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IndexStatus",
                table: "Chapters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IndexStatus",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IndexStatus",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "IndexStatus",
                table: "Books");
        }
    }
}
