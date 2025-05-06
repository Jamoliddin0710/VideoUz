using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class content : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ContentType",
                table: "Contents",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<long>(
                name: "fileId",
                table: "Contents",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contents_fileId",
                table: "Contents",
                column: "fileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_FileItem_fileId",
                table: "Contents",
                column: "fileId",
                principalTable: "FileItem",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_FileItem_fileId",
                table: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_Contents_fileId",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "fileId",
                table: "Contents");

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "Contents",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
