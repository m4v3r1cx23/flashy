using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flashy.API.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FlashCardMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Decks",
                schema: "flashy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deck", x => x.Id);
                    table.ForeignKey(
                        name: "FX_Deck_User_Id",
                        column: x => x.CreatedById,
                        principalSchema: "flashy",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "FlashCards",
                schema: "flashy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    Hint = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hash = table.Column<string>(type: "char(256)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashCard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlashCards_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "flashy",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FX_FlashCard_User_Id",
                        column: x => x.CreatedById,
                        principalSchema: "flashy",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Trials",
                schema: "flashy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastFlashCardIndex = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trial", x => x.Id);
                    table.ForeignKey(
                        name: "FX_Trial_User_Id",
                        column: x => x.UserId,
                        principalSchema: "flashy",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlashCardDeck",
                schema: "flashy",
                columns: table => new
                {
                    FlashCardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeckId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FX_FlashCardDeck_Deck_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashCardDeck", x => new { x.FlashCardId, x.DeckId });
                    table.ForeignKey(
                        name: "FK_FlashCardDeck_Decks_FX_FlashCardDeck_Deck_Id",
                        column: x => x.FX_FlashCardDeck_Deck_Id,
                        principalSchema: "flashy",
                        principalTable: "Decks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FX_FlashCardDeck_FlashCard_Id",
                        column: x => x.FlashCardId,
                        principalSchema: "flashy",
                        principalTable: "FlashCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                schema: "flashy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FlashCardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FX_Answer_FlashCard_Id",
                        column: x => x.FlashCardId,
                        principalSchema: "flashy",
                        principalTable: "FlashCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FX_Answer_Trial_Id",
                        column: x => x.TrialId,
                        principalSchema: "flashy",
                        principalTable: "Trials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlashCardTrial",
                schema: "flashy",
                columns: table => new
                {
                    FlashCardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashCardTrial", x => new { x.FlashCardId, x.TrialId });
                    table.ForeignKey(
                        name: "FX_FlashCardTrial_FlashCard_Id",
                        column: x => x.FlashCardId,
                        principalSchema: "flashy",
                        principalTable: "FlashCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FX_FlashCardTrial_Trial_Id",
                        column: x => x.TrialId,
                        principalSchema: "flashy",
                        principalTable: "Trials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answer_FlashCardId",
                schema: "flashy",
                table: "Answers",
                column: "FlashCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_TrialId",
                schema: "flashy",
                table: "Answers",
                column: "TrialId");

            migrationBuilder.CreateIndex(
                name: "IX_Deck_Category",
                schema: "flashy",
                table: "Decks",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Deck_CreatedAt",
                schema: "flashy",
                table: "Decks",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Deck_Name",
                schema: "flashy",
                table: "Decks",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Deck_UpdatedAt",
                schema: "flashy",
                table: "Decks",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Decks_CreatedById",
                schema: "flashy",
                table: "Decks",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FlashCardDeck_FX_FlashCardDeck_Deck_Id",
                schema: "flashy",
                table: "FlashCardDeck",
                column: "FX_FlashCardDeck_Deck_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FlashCard_Hash",
                schema: "flashy",
                table: "FlashCards",
                column: "Hash");

            migrationBuilder.CreateIndex(
                name: "IX_FlashCards_CreatedById",
                schema: "flashy",
                table: "FlashCards",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FlashCards_UserId",
                schema: "flashy",
                table: "FlashCards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashCardTrial_TrialId",
                schema: "flashy",
                table: "FlashCardTrial",
                column: "TrialId");

            migrationBuilder.CreateIndex(
                name: "IX_Trial_StartedAt",
                schema: "flashy",
                table: "Trials",
                column: "StartedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Trial_UserId",
                schema: "flashy",
                table: "Trials",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers",
                schema: "flashy");

            migrationBuilder.DropTable(
                name: "FlashCardDeck",
                schema: "flashy");

            migrationBuilder.DropTable(
                name: "FlashCardTrial",
                schema: "flashy");

            migrationBuilder.DropTable(
                name: "Decks",
                schema: "flashy");

            migrationBuilder.DropTable(
                name: "FlashCards",
                schema: "flashy");

            migrationBuilder.DropTable(
                name: "Trials",
                schema: "flashy");
        }
    }
}
