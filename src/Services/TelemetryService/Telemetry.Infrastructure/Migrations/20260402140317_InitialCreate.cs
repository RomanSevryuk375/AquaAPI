using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Telemetry.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "sensors",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                controller_id = table.Column<Guid>(type: "uuid", nullable: false),
                type = table.Column<int>(type: "integer", nullable: false),
                unit = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                last_value = table.Column<double>(type: "double precision", precision: 10, scale: 4, nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_sensors", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "telemetry_data",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                sensor_id = table.Column<Guid>(type: "uuid", nullable: false),
                value = table.Column<double>(type: "double precision", precision: 10, scale: 4, nullable: false),
                external_message_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                recorded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_telemetry_data", x => x.id);
                table.ForeignKey(
                    name: "fk_telemetry_data_sensors_sensor_id",
                    column: x => x.sensor_id,
                    principalTable: "sensors",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_telemetry_data_external_message_id",
            table: "telemetry_data",
            column: "external_message_id",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_telemetry_data_sensor_id_recorded_at",
            table: "telemetry_data",
            columns: new[] { "sensor_id", "recorded_at" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "telemetry_data");

        migrationBuilder.DropTable(
            name: "sensors");
    }
}
