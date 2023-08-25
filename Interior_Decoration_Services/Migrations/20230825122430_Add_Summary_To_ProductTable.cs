using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InteriorDecorationServices.Migrations
{
    /// <inheritdoc />
    public partial class AddSummaryToProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Summary",
                table: "products");
        }
    }
}
