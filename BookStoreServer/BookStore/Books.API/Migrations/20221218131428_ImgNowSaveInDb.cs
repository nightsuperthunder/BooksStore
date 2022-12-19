using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Books.API.Migrations
{
    /// <inheritdoc />
    public partial class ImgNowSaveInDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "PreviewImgPath",
                table: "Books",
                newName: "PreviewImg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PreviewImg",
                table: "Books",
                newName: "PreviewImgPath");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Books",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
