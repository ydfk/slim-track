using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlimTrack.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameToJinAndGongJin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeightG",
                table: "WeightEntries");

            migrationBuilder.RenameColumn(
                name: "WeightKg",
                table: "WeightEntries",
                newName: "WeightGongJin");

            migrationBuilder.AddColumn<decimal>(
                name: "WeightJin",
                table: "WeightEntries",
                type: "TEXT",
                precision: 6,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeightJin",
                table: "WeightEntries");

            migrationBuilder.RenameColumn(
                name: "WeightGongJin",
                table: "WeightEntries",
                newName: "WeightKg");

            migrationBuilder.AddColumn<decimal>(
                name: "WeightG",
                table: "WeightEntries",
                type: "TEXT",
                precision: 7,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
