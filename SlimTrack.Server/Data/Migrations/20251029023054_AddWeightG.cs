using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlimTrack.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWeightG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "WeightG",
                table: "WeightEntries",
                type: "TEXT",
                precision: 7,
                scale: 0,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeightG",
                table: "WeightEntries");
        }
    }
}
