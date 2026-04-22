using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserAndAquariumOwnership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_users_email",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "is_notify_enable",
                table: "users",
                newName: "is_notify_enabled");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_reminders_aquarium_id",
                table: "reminders",
                column: "aquarium_id");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_aquarium_id",
                table: "notifications",
                column: "aquarium_id");

            migrationBuilder.AddForeignKey(
                name: "fk_aquariums_users_user_id",
                table: "aquariums",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_maintenance_logs_aquariums_aquarium_id",
                table: "maintenance_logs",
                column: "aquarium_id",
                principalTable: "aquariums",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_maintenance_logs_users_user_id",
                table: "maintenance_logs",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_notifications_aquariums_aquarium_id",
                table: "notifications",
                column: "aquarium_id",
                principalTable: "aquariums",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_notifications_users_user_id",
                table: "notifications",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_reminders_aquariums_aquarium_id",
                table: "reminders",
                column: "aquarium_id",
                principalTable: "aquariums",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_reminders_users_user_id",
                table: "reminders",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_aquariums_users_user_id",
                table: "aquariums");

            migrationBuilder.DropForeignKey(
                name: "fk_maintenance_logs_aquariums_aquarium_id",
                table: "maintenance_logs");

            migrationBuilder.DropForeignKey(
                name: "fk_maintenance_logs_users_user_id",
                table: "maintenance_logs");

            migrationBuilder.DropForeignKey(
                name: "fk_notifications_aquariums_aquarium_id",
                table: "notifications");

            migrationBuilder.DropForeignKey(
                name: "fk_notifications_users_user_id",
                table: "notifications");

            migrationBuilder.DropForeignKey(
                name: "fk_reminders_aquariums_aquarium_id",
                table: "reminders");

            migrationBuilder.DropForeignKey(
                name: "fk_reminders_users_user_id",
                table: "reminders");

            migrationBuilder.DropIndex(
                name: "ix_users_email",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_reminders_aquarium_id",
                table: "reminders");

            migrationBuilder.DropIndex(
                name: "ix_notifications_aquarium_id",
                table: "notifications");

            migrationBuilder.RenameColumn(
                name: "is_notify_enabled",
                table: "users",
                newName: "is_notify_enable");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email");
        }
    }
}
