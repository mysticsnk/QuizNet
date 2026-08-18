using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizClient.Migrations
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

            migrationBuilder.DropIndex(
                name: "IX_QuestionOptions_MultiChoiceQuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropIndex(
                name: "IX_QuestionOptions_SingleChoiceQuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropColumn(
                name: "MultiChoiceQuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropColumn(
                name: "SingleChoiceQuestionId",
                table: "QuestionOptions");

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "QuestionOptions",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_QuestionId",
                table: "QuestionOptions",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropIndex(
                name: "IX_QuestionOptions_QuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "QuestionOptions");

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

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_MultiChoiceQuestionId",
                table: "QuestionOptions",
                column: "MultiChoiceQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_SingleChoiceQuestionId",
                table: "QuestionOptions",
                column: "SingleChoiceQuestionId");

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
        }
    }
}
