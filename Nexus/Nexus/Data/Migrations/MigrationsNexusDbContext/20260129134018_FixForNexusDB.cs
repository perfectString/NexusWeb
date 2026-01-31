using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexus.Migrations
{
    /// <inheritdoc />
    public partial class FixForNexusDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
