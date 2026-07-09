using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizServer.Migrations
{
    /// <inheritdoc />
    public partial class FixedQuestionClassQuizInfoFromFieldsToProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParticipantQuestionResults");

            migrationBuilder.DropTable(
                name: "CheckAnswerResult");

            migrationBuilder.DropTable(
                name: "ParticipantResults");

            migrationBuilder.DropTable(
                name: "Participant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Answer",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "AnswerText",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "SelectedOptionIds",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "TrueFalseAnswer_SelectedOptionId",
                table: "Answer");

            migrationBuilder.RenameTable(
                name: "Answer",
                newName: "TrueFalseAnswers");

            migrationBuilder.AlterColumn<Guid>(
                name: "SelectedOptionId",
                table: "TrueFalseAnswers",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrueFalseAnswers",
                table: "TrueFalseAnswers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MultiChoiceAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SelectedOptionIds = table.Column<string>(type: "TEXT", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    QuestionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TimeSpent = table.Column<TimeSpan>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiChoiceAnswers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShortTextAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AnswerText = table.Column<string>(type: "TEXT", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    QuestionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TimeSpent = table.Column<TimeSpan>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortTextAnswers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SingleChoiceAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SelectedOptionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    QuestionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TimeSpent = table.Column<TimeSpan>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleChoiceAnswers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MultiChoiceAnswers");

            migrationBuilder.DropTable(
                name: "ShortTextAnswers");

            migrationBuilder.DropTable(
                name: "SingleChoiceAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrueFalseAnswers",
                table: "TrueFalseAnswers");

            migrationBuilder.RenameTable(
                name: "TrueFalseAnswers",
                newName: "Answer");

            migrationBuilder.AlterColumn<Guid>(
                name: "SelectedOptionId",
                table: "Answer",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "AnswerText",
                table: "Answer",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Answer",
                type: "TEXT",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SelectedOptionIds",
                table: "Answer",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TrueFalseAnswer_SelectedOptionId",
                table: "Answer",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Answer",
                table: "Answer",
                column: "Id");

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
                    AccountId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CurrentQuestionIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Points = table.Column<int>(type: "INTEGER", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: false)
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
                    Place = table.Column<int>(type: "INTEGER", nullable: true),
                    TotalScore = table.Column<int>(type: "INTEGER", nullable: false)
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
                    AnswerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AnswerResultId = table.Column<Guid>(type: "TEXT", nullable: false),
                    QuestionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ParticipantResultId = table.Column<Guid>(type: "TEXT", nullable: false)
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
    }
}
