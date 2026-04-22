using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Control.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "aquariums",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    controller_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_aquariums", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "automation_rules",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    aquarium_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sensor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    relay_id = table.Column<Guid>(type: "uuid", nullable: false),
                    condition = table.Column<int>(type: "integer", nullable: false),
                    threshold = table.Column<double>(type: "double precision", precision: 18, scale: 2, nullable: false),
                    hysteresis = table.Column<double>(type: "double precision", precision: 18, scale: 2, nullable: false),
                    action = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_automation_rules", x => x.id);
                    table.ForeignKey(
                        name: "fk_automation_rules_aquariums_aquarium_id",
                        column: x => x.aquarium_id,
                        principalTable: "aquariums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "relays",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    aquarium_id = table.Column<Guid>(type: "uuid", nullable: false),
                    purpose = table.Column<int>(type: "integer", nullable: false),
                    is_manual = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_relays", x => x.id);
                    table.ForeignKey(
                        name: "fk_relays_aquariums_aquarium_id",
                        column: x => x.aquarium_id,
                        principalTable: "aquariums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "schedules",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    aquarium_id = table.Column<Guid>(type: "uuid", nullable: false),
                    relay_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cron_expression = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    duration_min = table.Column<double>(type: "double precision", nullable: false),
                    is_fade_mode = table.Column<bool>(type: "boolean", nullable: false),
                    is_enable = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_schedules", x => x.id);
                    table.ForeignKey(
                        name: "fk_schedules_aquariums_aquarium_id",
                        column: x => x.aquarium_id,
                        principalTable: "aquariums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sensors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    aquarium_id = table.Column<Guid>(type: "uuid", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sensors", x => x.id);
                    table.ForeignKey(
                        name: "fk_sensors_aquariums_aquarium_id",
                        column: x => x.aquarium_id,
                        principalTable: "aquariums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vacations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    aquarium_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    calculated_feed = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vacations", x => x.id);
                    table.ForeignKey(
                        name: "fk_vacations_aquariums_aquarium_id",
                        column: x => x.aquarium_id,
                        principalTable: "aquariums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_aquariums_controller_id",
                table: "aquariums",
                column: "controller_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_automation_rules_aquarium_id",
                table: "automation_rules",
                column: "aquarium_id");

            migrationBuilder.CreateIndex(
                name: "ix_automation_rules_relay_id",
                table: "automation_rules",
                column: "relay_id");

            migrationBuilder.CreateIndex(
                name: "ix_automation_rules_sensor_id",
                table: "automation_rules",
                column: "sensor_id");

            migrationBuilder.CreateIndex(
                name: "ix_relays_aquarium_id",
                table: "relays",
                column: "aquarium_id");

            migrationBuilder.CreateIndex(
                name: "ix_schedules_aquarium_id",
                table: "schedules",
                column: "aquarium_id");

            migrationBuilder.CreateIndex(
                name: "ix_sensors_aquarium_id",
                table: "sensors",
                column: "aquarium_id");

            migrationBuilder.CreateIndex(
                name: "ix_vacations_aquarium_id",
                table: "vacations",
                column: "aquarium_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "automation_rules");

            migrationBuilder.DropTable(
                name: "relays");

            migrationBuilder.DropTable(
                name: "schedules");

            migrationBuilder.DropTable(
                name: "sensors");

            migrationBuilder.DropTable(
                name: "vacations");

            migrationBuilder.DropTable(
                name: "aquariums");
        }
    }
}
