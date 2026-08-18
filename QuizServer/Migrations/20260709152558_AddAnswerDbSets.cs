using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizServer.Migrations
{
    /// <inheritdoc />
    public partial class AddAnswerDbSets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    QuestionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TimeSpent = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                    SelectedOptionIds = table.Column<string>(type: "TEXT", nullable: true),
                    AnswerText = table.Column<string>(type: "TEXT", nullable: true),
                    SelectedOptionId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TrueFalseAnswer_SelectedOptionId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CheckAnswerResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsCorrect = table.Column<bool>(type: "INTEGER", nullable: false),
                    PointsGained = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckAnswerResult", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Participant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    Points = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentQuestionIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participant_UserAccounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ParticipantResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TotalScore = table.Column<int>(type: "INTEGER", nullable: false),
                    Place = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParticipantResults_Participant_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParticipantQuestionResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ParticipantResultId = table.Column<Guid>(type: "TEXT", nullable: false),
                    QuestionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AnswerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AnswerResultId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantQuestionResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParticipantQuestionResults_Answer_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParticipantQuestionResults_CheckAnswerResult_AnswerResultId",
                        column: x => x.AnswerResultId,
                        principalTable: "CheckAnswerResult",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParticipantQuestionResults_ParticipantResults_ParticipantResultId",
                        column: x => x.ParticipantResultId,
                        principalTable: "ParticipantResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParticipantQuestionResults_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Participant_AccountId",
                table: "Participant",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantQuestionResults_AnswerId",
                table: "ParticipantQuestionResults",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantQuestionResults_AnswerResultId",
                table: "ParticipantQuestionResults",
                column: "AnswerResultId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantQuestionResults_ParticipantResultId",
                table: "ParticipantQuestionResults",
                column: "ParticipantResultId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantQuestionResults_QuestionId",
                table: "ParticipantQuestionResults",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantResults_ParticipantId",
                table: "ParticipantResults",
                column: "ParticipantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParticipantQuestionResults");

            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "CheckAnswerResult");

            migrationBuilder.DropTable(
                name: "ParticipantResults");

            migrationBuilder.DropTable(
                name: "Participant");
        }
    }
}
