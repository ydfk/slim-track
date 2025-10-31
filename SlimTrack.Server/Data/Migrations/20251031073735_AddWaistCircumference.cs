﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlimTrack.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWaistCircumference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "WaistCircumference",
                table: "WeightEntries",
                type: "TEXT",
                precision: 5,
                scale: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WaistCircumference",
                table: "WeightEntries");
        }
    }
}
