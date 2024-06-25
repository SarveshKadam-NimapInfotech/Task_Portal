using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskPortal.API.Migrations
{
    /// <inheritdoc />
    public partial class ReminderChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasBeenSent",
                table: "Reminders",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasBeenSent",
                table: "Reminders");
        }
    }
}
