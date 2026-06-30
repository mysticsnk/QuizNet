using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizServer.Migrations
{
    /// <inheritdoc />
    public partial class AddQuizTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionOptions_Questions_MultipleChoiceQuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionOptions_Questions_SingleChoiceQuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropIndex(
                name: "IX_QuestionOptions_MultipleChoiceQuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropIndex(
                name: "IX_QuestionOptions_SingleChoiceQuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropColumn(
                name: "MultipleChoiceQuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropColumn(
                name: "SingleChoiceQuestionId",
                table: "QuestionOptions");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Quizzes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Quizzes");

            migrationBuilder.AddColumn<Guid>(
                name: "MultipleChoiceQuestionId",
                table: "QuestionOptions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SingleChoiceQuestionId",
                table: "QuestionOptions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_MultipleChoiceQuestionId",
                table: "QuestionOptions",
                column: "MultipleChoiceQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_SingleChoiceQuestionId",
                table: "QuestionOptions",
                column: "SingleChoiceQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionOptions_Questions_MultipleChoiceQuestionId",
                table: "QuestionOptions",
                column: "MultipleChoiceQuestionId",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionOptions_Questions_SingleChoiceQuestionId",
                table: "QuestionOptions",
                column: "SingleChoiceQuestionId",
                principalTable: "Questions",
                principalColumn: "Id");
        }
    }
}
