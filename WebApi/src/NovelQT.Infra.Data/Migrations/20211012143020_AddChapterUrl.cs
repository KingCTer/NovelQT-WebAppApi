using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovelQT.Infra.Data.Migrations
{
    public partial class AddChapterUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Chapters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "Books");
        }
    }
}
