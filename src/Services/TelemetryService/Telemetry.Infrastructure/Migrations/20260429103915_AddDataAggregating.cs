using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Telemetry.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDataAggregating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_telemetry_raw_data_is_aggregated",
                table: "telemetry_raw_data");

            migrationBuilder.DropIndex(
                name: "ix_telemetry_aggregate_data_is_aggregated",
                table: "telemetry_aggregate_data");

            migrationBuilder.CreateIndex(
                name: "ix_telemetry_raw_data_is_aggregated",
                table: "telemetry_raw_data",
                column: "is_aggregated");

            migrationBuilder.CreateIndex(
                name: "ix_telemetry_aggregate_data_is_aggregated",
                table: "telemetry_aggregate_data",
                column: "is_aggregated");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_telemetry_raw_data_is_aggregated",
                table: "telemetry_raw_data");

            migrationBuilder.DropIndex(
                name: "ix_telemetry_aggregate_data_is_aggregated",
                table: "telemetry_aggregate_data");

            migrationBuilder.CreateIndex(
                name: "ix_telemetry_raw_data_is_aggregated",
                table: "telemetry_raw_data",
                column: "is_aggregated",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_telemetry_aggregate_data_is_aggregated",
                table: "telemetry_aggregate_data",
                column: "is_aggregated",
                unique: true);
        }
    }
}
