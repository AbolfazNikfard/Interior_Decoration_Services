using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InteriorDecorationServices.Migrations
{
    /// <inheritdoc />
    public partial class EditCartTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Number",
                table: "carts",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "carts",
                newName: "Number");
        }
    }
}
