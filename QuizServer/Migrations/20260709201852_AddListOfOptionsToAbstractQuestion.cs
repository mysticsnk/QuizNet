using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizServer.Migrations
{
    /// <inheritdoc />
    public partial class AddListOfOptionsToAbstractQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionOptions_Questions_MultiChoiceQuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionOptions_Questions_SingleChoiceQuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionOptions_Questions_TrueFalseQuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropIndex(
                name: "IX_QuestionOptions_MultiChoiceQuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropIndex(
                name: "IX_QuestionOptions_SingleChoiceQuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropIndex(
                name: "IX_QuestionOptions_TrueFalseQuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropColumn(
                name: "MultiChoiceQuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropColumn(
                name: "SingleChoiceQuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropColumn(
                name: "TrueFalseQuestionId",
                table: "QuestionOptions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MultiChoiceQuestionId",
                table: "QuestionOptions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SingleChoiceQuestionId",
                table: "QuestionOptions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TrueFalseQuestionId",
                table: "QuestionOptions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_MultiChoiceQuestionId",
                table: "QuestionOptions",
                column: "MultiChoiceQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_SingleChoiceQuestionId",
                table: "QuestionOptions",
                column: "SingleChoiceQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_TrueFalseQuestionId",
                table: "QuestionOptions",
                column: "TrueFalseQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionOptions_Questions_MultiChoiceQuestionId",
                table: "QuestionOptions",
                column: "MultiChoiceQuestionId",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionOptions_Questions_SingleChoiceQuestionId",
                table: "QuestionOptions",
                column: "SingleChoiceQuestionId",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionOptions_Questions_TrueFalseQuestionId",
                table: "QuestionOptions",
                column: "TrueFalseQuestionId",
                principalTable: "Questions",
                principalColumn: "Id");
        }
    }
}
