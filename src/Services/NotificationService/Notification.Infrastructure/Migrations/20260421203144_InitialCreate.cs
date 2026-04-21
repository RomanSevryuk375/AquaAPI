using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Infrastructure.Migrations
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
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_aquariums", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "maintenance_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    aquarium_id = table.Column<Guid>(type: "uuid", nullable: false),
                    action_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ph_level = table.Column<double>(type: "double precision", precision: 4, scale: 2, nullable: true),
                    kh_level = table.Column<double>(type: "double precision", precision: 4, scale: 2, nullable: true),
                    no3level = table.Column<double>(type: "double precision", precision: 4, scale: 2, nullable: true),
                    notes = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_maintenance_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    aquarium_id = table.Column<Guid>(type: "uuid", nullable: false),
                    level = table.Column<int>(type: "integer", nullable: false),
                    message = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    is_read = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_published = table.Column<bool>(type: "boolean", nullable: false),
                    published_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    retry_count = table.Column<int>(type: "integer", nullable: false),
                    failure_reason = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notifications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reminders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    aquarium_id = table.Column<Guid>(type: "uuid", nullable: false),
                    task_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    interval_days = table.Column<int>(type: "integer", nullable: false),
                    last_done_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_notified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    next_due_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reminders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    phone_number = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    email_enable = table.Column<bool>(type: "boolean", nullable: false),
                    tg_enable = table.Column<bool>(type: "boolean", nullable: false),
                    telegram_chat_id = table.Column<long>(type: "bigint", nullable: true),
                    is_notify_enable = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_aquariums_user_id",
                table: "aquariums",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_maintenance_logs_aquarium_id",
                table: "maintenance_logs",
                column: "aquarium_id");

            migrationBuilder.CreateIndex(
                name: "ix_maintenance_logs_user_id",
                table: "maintenance_logs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_is_read",
                table: "notifications",
                column: "is_read");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_level",
                table: "notifications",
                column: "level");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_user_id",
                table: "notifications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_reminders_next_due_at",
                table: "reminders",
                column: "next_due_at");

            migrationBuilder.CreateIndex(
                name: "ix_reminders_user_id",
                table: "reminders",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aquariums");

            migrationBuilder.DropTable(
                name: "maintenance_logs");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "reminders");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
