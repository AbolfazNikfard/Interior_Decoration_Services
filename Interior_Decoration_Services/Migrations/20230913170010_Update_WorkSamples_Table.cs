using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InteriorDecorationServices.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWorkSamplesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkSamples_groups_groupId",
                table: "WorkSamples");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkSamples",
                table: "WorkSamples");

            migrationBuilder.RenameTable(
                name: "WorkSamples",
                newName: "workSamples");

            migrationBuilder.RenameIndex(
                name: "IX_WorkSamples_groupId",
                table: "workSamples",
                newName: "IX_workSamples_groupId");

            migrationBuilder.AddColumn<DateTime>(
                name: "createdAt",
                table: "workSamples",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "editedAt",
                table: "workSamples",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_workSamples",
                table: "workSamples",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_workSamples_groups_groupId",
                table: "workSamples",
                column: "groupId",
                principalTable: "groups",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_workSamples_groups_groupId",
                table: "workSamples");

            migrationBuilder.DropPrimaryKey(
                name: "PK_workSamples",
                table: "workSamples");

            migrationBuilder.DropColumn(
                name: "createdAt",
                table: "workSamples");

            migrationBuilder.DropColumn(
                name: "editedAt",
                table: "workSamples");

            migrationBuilder.RenameTable(
                name: "workSamples",
                newName: "WorkSamples");

            migrationBuilder.RenameIndex(
                name: "IX_workSamples_groupId",
                table: "WorkSamples",
                newName: "IX_WorkSamples_groupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkSamples",
                table: "WorkSamples",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSamples_groups_groupId",
                table: "WorkSamples",
                column: "groupId",
                principalTable: "groups",
                principalColumn: "id");
        }
    }
}
