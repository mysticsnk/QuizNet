using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizServer.Migrations
{
    /// <inheritdoc />
    public partial class RenameUserPasswordToPasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "UserAccounts",
                newName: "PasswordHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "UserAccounts",
                newName: "Password");
        }
    }
}
