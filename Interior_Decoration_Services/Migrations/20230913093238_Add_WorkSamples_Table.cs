using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InteriorDecorationServices.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkSamplesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkSamples",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    groupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSamples", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkSamples_groups_groupId",
                        column: x => x.groupId,
                        principalTable: "groups",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkSamples_groupId",
                table: "WorkSamples",
                column: "groupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkSamples");
        }
    }
}
