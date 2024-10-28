using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientSWH.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Pid",
                table: "history_pkg",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_history_pkg_Pid",
                table: "history_pkg",
                column: "Pid");

            migrationBuilder.AddForeignKey(
                name: "FK_history_pkg_packages_Pid",
                table: "history_pkg",
                column: "Pid",
                principalTable: "packages",
                principalColumn: "pid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_history_pkg_packages_Pid",
                table: "history_pkg");

            migrationBuilder.DropIndex(
                name: "IX_history_pkg_Pid",
                table: "history_pkg");

            migrationBuilder.AlterColumn<int>(
                name: "Pid",
                table: "history_pkg",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
