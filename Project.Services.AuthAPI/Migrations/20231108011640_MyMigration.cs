using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Services.AuthAPI.Migrations
{
    /// <inheritdoc />
    public partial class MyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c1eb4373-b095-458b-be43-5ed0e158c28f", "AQAAAAIAAYagAAAAEFyfcQIOO9FeSW12bvulEH1uBxjp2MARqRthnGfqlSWnyMitvr6px1EROmP2xmqPdQ==", "fbe045d1-e43f-4227-8070-469db37a3f08" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7f07ad31-e3cd-4d9d-82aa-1ebf678d5d5c", "AQAAAAIAAYagAAAAENt+BjREySNVMxEW2AZtjK0PZyQ/RoXOcmJdvl4sLNn6tMnsiknypfVfyLqVRXOLOg==", "6274af11-06d5-41f5-8065-c43240ed7282" });
        }
    }
}
