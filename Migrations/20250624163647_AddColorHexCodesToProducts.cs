using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cookware_react_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddColorHexCodesToProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColorHexCodes",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorHexCodes",
                table: "Products");
        }
    }
}
