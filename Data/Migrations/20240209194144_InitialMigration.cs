using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flashy.Data.Context
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "flashy");

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "flashy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    NormalizedName = table.Column<string>(type: "varchar(256)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(256)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "flashy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", nullable: false),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(320)", nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(24)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                schema: "flashy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "varchar(128)", nullable: false),
                    ClaimValue = table.Column<string>(type: "varchar(128)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "flashy",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Decks",
                schema: "flashy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Decks_CreatedBy",
                        column: x => x.CreatedById,
                        principalSchema: "flashy",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlashCards",
                schema: "flashy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Front = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    NormalizedFront = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    Back = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    NormalizedBack = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    Hint = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlashCards_CreatedBy",
                        column: x => x.CreatedById,
                        principalSchema: "flashy",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                schema: "flashy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "varchar(128)", nullable: false),
                    ClaimValue = table.Column<string>(type: "varchar(128)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "flashy",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                schema: "flashy",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(128)", nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(128)", nullable: false),
                    NormalizedProviderDisplayName = table.Column<string>(type: "varchar(128)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "flashy",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "flashy",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "flashy",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "flashy",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                schema: "flashy",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(256)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "flashy",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersDecks",
                schema: "flashy",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeckId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersDecks", x => new { x.UserId, x.DeckId });
                    table.ForeignKey(
                        name: "FK_UsersDecks_Decks",
                        column: x => x.DeckId,
                        principalSchema: "flashy",
                        principalTable: "Decks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UsersDecks_Users",
                        column: x => x.UserId,
                        principalSchema: "flashy",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DecksFlashCards",
                schema: "flashy",
                columns: table => new
                {
                    DeckId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FlashCardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FlashCardsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecksFlashCards", x => new { x.DeckId, x.FlashCardId });
                    table.ForeignKey(
                        name: "FK_DecksFlashCards_Decks",
                        column: x => x.DeckId,
                        principalSchema: "flashy",
                        principalTable: "Decks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DecksFlashCards_FlashCards",
                        column: x => x.FlashCardId,
                        principalSchema: "flashy",
                        principalTable: "FlashCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DecksFlashCards_FlashCards_FlashCardsId",
                        column: x => x.FlashCardsId,
                        principalSchema: "flashy",
                        principalTable: "FlashCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Decks_CreatedAt",
                schema: "flashy",
                table: "Decks",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Decks_CreatedById",
                schema: "flashy",
                table: "Decks",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Decks_NormalizedName",
                schema: "flashy",
                table: "Decks",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DecksFlashCards_FlashCardId",
                schema: "flashy",
                table: "DecksFlashCards",
                column: "FlashCardId");

            migrationBuilder.CreateIndex(
                name: "IX_DecksFlashCards_FlashCardsId",
                schema: "flashy",
                table: "DecksFlashCards",
                column: "FlashCardsId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashCards_CreatedAt",
                schema: "flashy",
                table: "FlashCards",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FlashCards_CreatedBy",
                schema: "flashy",
                table: "FlashCards",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FlashCards_NormalizedBack",
                schema: "flashy",
                table: "FlashCards",
                column: "NormalizedBack",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlashCards_NormalizedFront",
                schema: "flashy",
                table: "FlashCards",
                column: "NormalizedFront",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                schema: "flashy",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                schema: "flashy",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_NormalizedName",
                schema: "flashy",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                schema: "flashy",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_NormalizedProviderDisplayName",
                schema: "flashy",
                table: "UserLogins",
                column: "NormalizedProviderDisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_ProviderDisplayName",
                schema: "flashy",
                table: "UserLogins",
                column: "ProviderDisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                schema: "flashy",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "flashy",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "flashy",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_NormalizedEmail",
                schema: "flashy",
                table: "Users",
                column: "NormalizedEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_NormalizedUserName",
                schema: "flashy",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                schema: "flashy",
                table: "Users",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersDecks_DeckId",
                schema: "flashy",
                table: "UsersDecks",
                column: "DeckId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DecksFlashCards",
                schema: "flashy");

            migrationBuilder.DropTable(
                name: "RoleClaims",
                schema: "flashy");

            migrationBuilder.DropTable(
                name: "UserClaims",
                schema: "flashy");

            migrationBuilder.DropTable(
                name: "UserLogins",
                schema: "flashy");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "flashy");

            migrationBuilder.DropTable(
                name: "UsersDecks",
                schema: "flashy");

            migrationBuilder.DropTable(
                name: "UserTokens",
                schema: "flashy");

            migrationBuilder.DropTable(
                name: "FlashCards",
                schema: "flashy");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "flashy");

            migrationBuilder.DropTable(
                name: "Decks",
                schema: "flashy");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "flashy");
        }
    }
}
