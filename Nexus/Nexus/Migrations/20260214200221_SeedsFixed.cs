using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nexus.Migrations
{
    /// <inheritdoc />
    public partial class SeedsFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 27, "10" });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 29, "10" });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 30, "10" });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 21, "5" });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 22, "5" });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 23, "5" });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 13, "6" });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 14, "6" });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 26, "6" });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 2, "7" });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 22, "7" });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 24, "7" });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 2, "8" });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 15, "8" });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 19, "8" });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 16, "9" });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 17, "9" });

            migrationBuilder.DeleteData(
                table: "ProfileInterests",
                keyColumns: new[] { "InterestId", "ProfileId" },
                keyValues: new object[] { 19, "9" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "10");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9");

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

            migrationBuilder.InsertData(
                table: "Quests",
                columns: new[] { "Id", "Description", "QuestInitiatorId", "Title" },
                values: new object[,]
                {
                    { 1, "Join us for some fishing, cooking and camping near the river!", "4", "Camping near the river" },
                    { 2, "Im looking for people to play cs with so i'd love for us to form a squad!", "3", "Gaming night" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "57edb2a2-40ba-4333-87ec-a8aa257114a1", "464e6165-20cb-4f6a-87a0-92a5145604dd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "36baf2c6-a5d1-4afc-9aba-e70a264bf108", "6fd1dc24-6500-43ec-b170-a29398f8d3d5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "77cd4472-df81-4bb8-9c18-4c03ccc75287", "c45d763f-405d-4518-889a-53ade5736aef" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "9007f133-32c0-4164-932c-714b92d7f522", "740c6e33-910a-4266-804a-dc87a77d518b" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Age", "Bio", "City", "ConcurrencyStamp", "DesiredConnection", "DisplayName", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "10", 0, 19, "I would like to find a local band. Can play bass pretty good!", "London", "fa10b5d2-ac01-4861-a81c-b3e087656308", 2, "Dan", null, false, false, null, null, null, null, null, false, "323f1868-bc6f-4680-bc50-571bf490fe9c", false, null },
                    { "5", 0, 27, "Let's hang out?", "Sofia", "c4051f7e-5c90-42cf-9dc8-9948d0881d0b", 0, "Peter", null, false, false, null, null, null, null, null, false, "4e49815c-7cc8-4d85-9f5f-06523ece4aca", false, null },
                    { "6", 0, 44, null, "Rome", "25e87d35-962b-4bd1-afaf-fc732a9f0180", 1, "Emma", null, false, false, null, null, null, null, null, false, "0f021080-4e30-4e17-ba6d-40ca2ee79502", false, null },
                    { "7", 0, 20, "Heavy metal!!", "Sofia", "b71069c9-e4bb-4050-a8d4-feca571fec9a", 2, "Luca", null, false, false, null, null, null, null, null, false, "a5248105-54eb-4221-a84c-67bab355d631", false, null },
                    { "8", 0, 26, "Recommend me new music", "Madrid", "cc98755e-ed15-4884-aab5-caa12a563543", 0, "Alexandra", null, false, false, null, null, null, null, null, false, "c25ebdef-8ddc-4283-8c5a-c71e1499f443", false, null },
                    { "9", 0, 33, "Lets travel the world together!", "London", "efae049c-b640-42f1-a1c3-d539dc46b4b1", 1, "Olivia", null, false, false, null, null, null, null, null, false, "9abfb043-fa21-45e7-b0ca-ebfa03c23229", false, null }
                });

            migrationBuilder.InsertData(
                table: "ProfileInterests",
                columns: new[] { "InterestId", "ProfileId" },
                values: new object[,]
                {
                    { 27, "10" },
                    { 29, "10" },
                    { 30, "10" },
                    { 21, "5" },
                    { 22, "5" },
                    { 23, "5" },
                    { 13, "6" },
                    { 14, "6" },
                    { 26, "6" },
                    { 2, "7" },
                    { 22, "7" },
                    { 24, "7" },
                    { 2, "8" },
                    { 15, "8" },
                    { 19, "8" },
                    { 16, "9" },
                    { 17, "9" },
                    { 19, "9" }
                });
        }
    }
}
