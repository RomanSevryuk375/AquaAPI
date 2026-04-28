using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Telemetry.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddIsDataDelayed : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "is_data_delayed",
            table: "sensors",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<int>(
            name: "state",
            table: "sensors",
            type: "integer",
            nullable: false,
            defaultValue: 0);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "is_data_delayed",
            table: "sensors");

        migrationBuilder.DropColumn(
            name: "state",
            table: "sensors");
    }
}
