using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nexus.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR(450)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(85)", maxLength: 85, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "nvarchar(85)", maxLength: 85, nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DesiredConnection = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Interests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "NVARCHAR(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "NVARCHAR(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "NVARCHAR(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "NVARCHAR(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FriendRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<string>(type: "NVARCHAR(450)", nullable: false),
                    RecieverId = table.Column<string>(type: "NVARCHAR(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendRequests_AspNetUsers_RecieverId",
                        column: x => x.RecieverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FriendRequests_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfileInterests",
                columns: table => new
                {
                    ProfileId = table.Column<string>(type: "NVARCHAR(450)", nullable: false),
                    InterestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileInterests", x => new { x.ProfileId, x.InterestId });
                    table.ForeignKey(
                        name: "FK_ProfileInterests_AspNetUsers_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileInterests_Interests_InterestId",
                        column: x => x.InterestId,
                        principalTable: "Interests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Age", "Bio", "City", "ConcurrencyStamp", "DesiredConnection", "DisplayName", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1", 0, 21, "New in the city and looking for new connections!", "Sofia", "9eee8c16-aa15-4b34-a8db-91c766573c6a", 0, "Alex", null, false, false, null, null, null, null, null, false, "699b3658-7d88-442f-acb8-0f868de5adda", false, null },
                    { "10", 0, 19, "I would like to find a local band. Can play bass pretty good!", "London", "cba69389-83db-45d3-8bcb-4e1a730927fe", 2, "Dan", null, false, false, null, null, null, null, null, false, "a5e8227c-84cf-4463-802f-f622b94f72ba", false, null },
                    { "2", 0, 30, "Looking for my person", "Berlin", "fcd4cd59-5e62-4cf1-a297-82e698acdcd2", 1, "Lidya", null, false, false, null, null, null, null, null, false, "cea40cf9-84ce-4353-b23d-ebbd825afbb4", false, null },
                    { "3", 0, 19, "Im heavy into gaming, i'd like to find people to play CS with!!!", "Madrid", "db9bfa19-73f1-4104-8abb-37f898e60970", 2, "Liam", null, false, false, null, null, null, null, null, false, "f4524962-99d9-4c49-92d6-0d76fa94d2bd", false, null },
                    { "4", 0, 31, "Work in the tech field.Into long night walks.", "London", "196d5ade-dcfc-4349-b7ed-b6888b769b41", 1, "Dean", null, false, false, null, null, null, null, null, false, "b992bbea-45d1-47a5-963f-5e06655bb4d5", false, null },
                    { "5", 0, 27, "Let's hang out?", "Sofia", "07c5bc61-2f8a-4bb3-acb2-89d770f49584", 0, "Peter", null, false, false, null, null, null, null, null, false, "52cc8469-9376-4e8b-9c64-07fa35bcffe0", false, null },
                    { "6", 0, 44, null, "Rome", "b80d85e3-d67f-4538-930a-b43e9937b11e", 1, "Emma", null, false, false, null, null, null, null, null, false, "f35279b8-552e-4d19-80ed-9a1e3d770b39", false, null },
                    { "7", 0, 20, "Heavy metal!!", "Sofia", "f454af75-fc20-47b6-ad6c-cbe09ae2c0af", 2, "Luca", null, false, false, null, null, null, null, null, false, "baffbe33-2d67-4d76-ad55-3e1f7b3b12e7", false, null },
                    { "8", 0, 26, "Recommend me new music", "Madrid", "d9286632-1fa3-4890-ab29-3d4f61e625ed", 0, "Alexandra", null, false, false, null, null, null, null, null, false, "6aa41cd4-68b2-4480-aa57-2d3d3d3822a6", false, null },
                    { "9", 0, 33, "Lets travel the world together!", "London", "2c00d0b3-b37c-40ef-944d-a5628885a4c3", 1, "Olivia", null, false, false, null, null, null, null, null, false, "05347c4c-39b4-4146-9917-604cb736949c", false, null }
                });

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
                table: "ProfileInterests",
                columns: new[] { "InterestId", "ProfileId" },
                values: new object[,]
                {
                    { 2, "1" },
                    { 5, "1" },
                    { 6, "1" },
                    { 27, "10" },
                    { 29, "10" },
                    { 30, "10" },
                    { 3, "2" },
                    { 6, "2" },
                    { 11, "2" },
                    { 4, "3" },
                    { 9, "3" },
                    { 16, "3" },
                    { 7, "4" },
                    { 15, "4" },
                    { 17, "4" },
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

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_RecieverId",
                table: "FriendRequests",
                column: "RecieverId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_SenderId",
                table: "FriendRequests",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInterests_InterestId",
                table: "ProfileInterests",
                column: "InterestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "FriendRequests");

            migrationBuilder.DropTable(
                name: "ProfileInterests");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Interests");
        }
    }
}
