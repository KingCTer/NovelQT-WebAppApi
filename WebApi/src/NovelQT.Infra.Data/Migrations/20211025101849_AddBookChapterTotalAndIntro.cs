using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovelQT.Infra.Data.Migrations
{
    public partial class AddBookChapterTotalAndIntro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChapterTotal",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Intro",
                table: "Books",
                type: "ntext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChapterTotal",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Intro",
                table: "Books");
        }
    }
}
