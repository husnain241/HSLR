using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSLR.Migrations
{
    /// <inheritdoc />
    public partial class AddPublicationImagesToPublication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image1Url",
                table: "Publications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image2Url",
                table: "Publications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image3Url",
                table: "Publications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image4Url",
                table: "Publications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image1Url",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "Image2Url",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "Image3Url",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "Image4Url",
                table: "Publications");
        }
    }
}
