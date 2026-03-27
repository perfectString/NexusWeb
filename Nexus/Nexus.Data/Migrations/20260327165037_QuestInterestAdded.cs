using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nexus.Data.Migrations
{
    /// <inheritdoc />
    public partial class QuestInterestAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestInterests",
                columns: table => new
                {
                    QuestId = table.Column<int>(type: "int", nullable: false),
                    InterestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestInterests", x => new { x.QuestId, x.InterestId });
                    table.ForeignKey(
                        name: "FK_QuestInterests_Interests_InterestId",
                        column: x => x.InterestId,
                        principalTable: "Interests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestInterests_Quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a1111111-1111-1111-1111-111111111111"),
                column: "ConcurrencyStamp",
                value: "97ad1874-62f6-4526-85b0-e7b89497c2bc");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a2222222-2222-2222-2222-222222222222"),
                column: "ConcurrencyStamp",
                value: "e1b50b85-eb63-4453-8c1b-9cc3d140fc9b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a3333333-3333-3333-3333-333333333333"),
                column: "ConcurrencyStamp",
                value: "28820cce-4121-4f09-bf53-57949c79009f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a4444444-4444-4444-4444-444444444444"),
                column: "ConcurrencyStamp",
                value: "b1fe8267-ba1d-4489-9941-63b9f5d6b1fb");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Age", "Bio", "City", "ConcurrencyStamp", "DesiredConnection", "DisplayName", "Email", "EmailConfirmed", "ExperiencePoints", "Level", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("a5555555-5555-5555-5555-555555555555"), 0, 24, "Love art galleries and coffee spots. Always up for creative activities!", "Paris", "a9d8c755-1eae-4aeb-98c7-d73b473ef323", 0, "Maya", null, false, 520, 6, false, null, null, null, null, null, false, null, false, null },
                    { new Guid("a6666666-6666-6666-6666-666666666666"), 0, 27, "Cycling enthusiast and fitness lover. Looking for active people.", "Amsterdam", "1c1e31b6-002e-42e3-9b25-aa3c064a464c", 2, "Noah", null, false, 950, 10, false, null, null, null, null, null, false, null, false, null },
                    { new Guid("a7777777-7777-7777-7777-777777777777"), 0, 28, "Foodie and traveler. Let’s explore new places together!", "Amsterdam", "3ff66ed6-147b-4396-a48d-a4411b65d5f4", 0, "Elena", null, false, 430, 5, false, null, null, null, null, null, false, null, false, null },
                    { new Guid("a8888888-8888-8888-8888-888888888888"), 0, 29, "Into tech, startups and hackathons. Building cool stuff.", "Sofia", "54d1efd7-f257-43a0-94a3-225d6a42787a", 2, "Victor", null, false, 1100, 12, false, null, null, null, null, null, false, null, false, null }
                });

            migrationBuilder.InsertData(
                table: "QuestInterests",
                columns: new[] { "InterestId", "QuestId" },
                values: new object[,]
                {
                    { 6, 1 },
                    { 15, 1 },
                    { 4, 2 },
                    { 21, 3 }
                });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 1);

            migrationBuilder.InsertData(
                table: "ProfileInterests",
                columns: new[] { "InterestId", "ProfileId" },
                values: new object[,]
                {
                    { 8, new Guid("a5555555-5555-5555-5555-555555555555") },
                    { 26, new Guid("a5555555-5555-5555-5555-555555555555") },
                    { 5, new Guid("a6666666-6666-6666-6666-666666666666") },
                    { 15, new Guid("a6666666-6666-6666-6666-666666666666") },
                    { 16, new Guid("a6666666-6666-6666-6666-666666666666") },
                    { 7, new Guid("a7777777-7777-7777-7777-777777777777") },
                    { 20, new Guid("a7777777-7777-7777-7777-777777777777") },
                    { 22, new Guid("a8888888-8888-8888-8888-888888888888") }
                });

            migrationBuilder.InsertData(
                table: "Quests",
                columns: new[] { "Id", "Description", "Difficulty", "QuestInitiatorId", "RewardXp", "Status", "Title" },
                values: new object[] { 4, "Looking for committed people to stay consistent and push limits for 30 days.", 2, new Guid("a6666666-6666-6666-6666-666666666666"), 200, 0, "30-Day Fitness Challenge" });

            migrationBuilder.InsertData(
                table: "QuestInterests",
                columns: new[] { "InterestId", "QuestId" },
                values: new object[,]
                {
                    { 5, 4 },
                    { 16, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestInterests_InterestId",
                table: "QuestInterests",
                column: "InterestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestInterests");

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 8, new Guid("a5555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 26, new Guid("a5555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 5, new Guid("a6666666-6666-6666-6666-666666666666") });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 15, new Guid("a6666666-6666-6666-6666-666666666666") });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 16, new Guid("a6666666-6666-6666-6666-666666666666") });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 7, new Guid("a7777777-7777-7777-7777-777777777777") });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 20, new Guid("a7777777-7777-7777-7777-777777777777") });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 22, new Guid("a8888888-8888-8888-8888-888888888888") });

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a5555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a6666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a7777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a8888888-8888-8888-8888-888888888888"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a1111111-1111-1111-1111-111111111111"),
                column: "ConcurrencyStamp",
                value: "9c609905-a329-4257-b01f-f9a833c5e5d9");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a2222222-2222-2222-2222-222222222222"),
                column: "ConcurrencyStamp",
                value: "c713a783-8d16-4201-86b7-1b0b7b00f78d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a3333333-3333-3333-3333-333333333333"),
                column: "ConcurrencyStamp",
                value: "312e8c34-7816-4298-84c2-bdd8c276973a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a4444444-4444-4444-4444-444444444444"),
                column: "ConcurrencyStamp",
                value: "472bca3e-d1e9-4581-b5c2-3f0a2ecc506e");

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 0);
        }
    }
}
