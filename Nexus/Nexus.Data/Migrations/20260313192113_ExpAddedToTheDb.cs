using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexus.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExpAddedToTheDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "Quests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExperiencePoints",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "ExperiencePoints", "Level", "SecurityStamp" },
                values: new object[] { "c472a15f-4db9-4e35-bfb7-a3b971adadc6", 200, 2, "f9ec9379-e878-42be-abaa-698931122e74" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "ExperiencePoints", "Level", "SecurityStamp" },
                values: new object[] { "bae18361-229c-49bd-865f-536b80d2cff6", 100, 1, "9f717c1a-6496-4c8d-bafa-6606da54d979" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "ExperiencePoints", "Level", "SecurityStamp" },
                values: new object[] { "b3b9e070-51d8-4b3e-bfe5-30e1cec3d066", 350, 3, "8bff6b3d-a48b-4feb-9fa6-966f46aa21f0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4",
                columns: new[] { "ConcurrencyStamp", "ExperiencePoints", "Level", "SecurityStamp" },
                values: new object[] { "ba46013a-6869-4007-b503-4e57eb10e683", 100, 1, "efb03d14-6241-405e-93a4-b88e2b87731a" });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                column: "Difficulty",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                column: "Difficulty",
                value: 0);

            migrationBuilder.InsertData(
                table: "Quests",
                columns: new[] { "Id", "Description", "Difficulty", "QuestInitiatorId", "Title" },
                values: new object[] { 3, "Let's clean the city!", 1, "1", "Community Work" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "ExperiencePoints",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "b46058b7-875d-47ac-ad71-cd2955575f33", "64b9e6eb-1a8c-40ba-b2ed-e5cf650f0012" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "0ed8a136-8961-45e6-ba6b-d11852d8e19d", "02795cf0-bbe8-4a83-b44a-451b347daa8f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "8612d5be-07e7-4353-b5ad-4605facff1be", "8fc3b581-2a7d-4316-9367-a6ad209a18e3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a60762f3-fa85-49cd-8147-98b3e30fd234", "c4ea5fd2-59a2-4955-a47d-04beddbf0a27" });
        }
    }
}
