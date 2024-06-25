using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskPortal.API.Migrations
{
    /// <inheritdoc />
    public partial class CategoriesWithDefault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create the Categories table
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            // Insert initial data into Categories
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Name" },
                values: new object[] { "Default Category" }
            );

            // Create the CategoryId column in Tasks table
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Tasks",
                nullable: false,
                defaultValue: 1
            );

            // Create the foreign key constraint
            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CategoryId",
                table: "Tasks",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Categories_CategoryId",
                table: "Tasks",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Categories_CategoryId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_CategoryId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
