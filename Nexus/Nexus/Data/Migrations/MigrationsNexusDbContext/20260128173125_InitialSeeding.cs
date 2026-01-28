using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nexus.Migrations
{
    /// <inheritdoc />
    public partial class InitialSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequests_Users_RecieverId",
                table: "FriendRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequests_Users_SenderId",
                table: "FriendRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersInterests_Interests_InterestId",
                table: "UsersInterests");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersInterests_Users_UserId",
                table: "UsersInterests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersInterests",
                table: "UsersInterests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "UsersInterests",
                newName: "UserInterest");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameIndex(
                name: "IX_UsersInterests_InterestId",
                table: "UserInterest",
                newName: "IX_UserInterest_InterestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInterest",
                table: "UserInterest",
                columns: new[] { "UserId", "InterestId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Interests",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Reading" },
                    { 2, "Music" },
                    { 3, "Movies" },
                    { 4, "Gaming" },
                    { 5, "Fitness" },
                    { 6, "Cooking" },
                    { 7, "Travelling" },
                    { 8, "Photography" },
                    { 9, "Animals" },
                    { 10, "Board Games" },
                    { 11, "Meditation" },
                    { 12, "Writing" },
                    { 13, "Education" },
                    { 14, "Languages" },
                    { 15, "Nature" },
                    { 16, "Hiking" },
                    { 17, "Camping" },
                    { 18, "Gardening" },
                    { 19, "Family" },
                    { 20, "Socializing" },
                    { 21, "Volunteering" },
                    { 22, "Technology" },
                    { 23, "News" },
                    { 24, "Politics" },
                    { 25, "Crafts" },
                    { 26, "Art" },
                    { 27, "Reading" },
                    { 28, "Drawing" },
                    { 29, "Fashion" },
                    { 30, "Driving" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Age", "Bio", "City", "DesiredConnection", "Name" },
                values: new object[,]
                {
                    { 1, 21, "New in the city and looking for new connections!", "Sofia", 0, "Alex" },
                    { 2, 30, "Looking for my person", "Berlin", 1, "Lidya" },
                    { 3, 19, "Im heavy into gaming, i'd like to find people to play CS with!!!", "Madrid", 2, "Liam" },
                    { 4, 31, "Work in the tech field.Into long night walks.", "London", 1, "Dean" },
                    { 5, 27, "Let's hang out?", "Sofia", 0, "Peter" },
                    { 6, 44, null, "Rome", 1, "Emma" },
                    { 7, 20, "Heavy metal!!", "Sofia", 2, "Luca" },
                    { 8, 26, "Recommend me new music", "Madrid", 0, "Alexandra" },
                    { 9, 33, "Lets travel the world together!", "London", 1, "Olivia" },
                    { 10, 19, "I would like to find a local band. Can play bass pretty good!", "London", 2, "Dan" }
                });

            migrationBuilder.InsertData(
                table: "UserInterest",
                columns: new[] { "InterestId", "UserId" },
                values: new object[,]
                {
                    { 2, 1 },
                    { 5, 1 },
                    { 6, 1 },
                    { 3, 2 },
                    { 6, 2 },
                    { 11, 2 },
                    { 4, 3 },
                    { 9, 3 },
                    { 16, 3 },
                    { 7, 4 },
                    { 15, 4 },
                    { 17, 4 },
                    { 21, 5 },
                    { 22, 5 },
                    { 23, 5 },
                    { 13, 6 },
                    { 14, 6 },
                    { 26, 6 },
                    { 2, 7 },
                    { 22, 7 },
                    { 24, 7 },
                    { 2, 8 },
                    { 15, 8 },
                    { 19, 8 },
                    { 16, 9 },
                    { 17, 9 },
                    { 19, 9 },
                    { 27, 10 },
                    { 29, 10 },
                    { 30, 10 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequests_User_RecieverId",
                table: "FriendRequests",
                column: "RecieverId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequests_User_SenderId",
                table: "FriendRequests",
                column: "SenderId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInterest_Interests_InterestId",
                table: "UserInterest",
                column: "InterestId",
                principalTable: "Interests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInterest_User_UserId",
                table: "UserInterest",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequests_User_RecieverId",
                table: "FriendRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequests_User_SenderId",
                table: "FriendRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInterest_Interests_InterestId",
                table: "UserInterest");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInterest_User_UserId",
                table: "UserInterest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInterest",
                table: "UserInterest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 6, 1 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 6, 2 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 11, 2 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 4, 3 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 9, 3 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 16, 3 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 7, 4 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 15, 4 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 17, 4 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 21, 5 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 22, 5 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 23, 5 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 13, 6 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 14, 6 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 26, 6 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 2, 7 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 22, 7 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 24, 7 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 2, 8 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 15, 8 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 19, 8 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 16, 9 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 17, 9 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 19, 9 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 27, 10 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 29, 10 });

            migrationBuilder.DeleteData(
                table: "UserInterest",
                keyColumns: new[] { "InterestId", "UserId" },
                keyValues: new object[] { 30, 10 });

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.RenameTable(
                name: "UserInterest",
                newName: "UsersInterests");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_UserInterest_InterestId",
                table: "UsersInterests",
                newName: "IX_UsersInterests_InterestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersInterests",
                table: "UsersInterests",
                columns: new[] { "UserId", "InterestId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequests_Users_RecieverId",
                table: "FriendRequests",
                column: "RecieverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequests_Users_SenderId",
                table: "FriendRequests",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersInterests_Interests_InterestId",
                table: "UsersInterests",
                column: "InterestId",
                principalTable: "Interests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersInterests_Users_UserId",
                table: "UsersInterests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
