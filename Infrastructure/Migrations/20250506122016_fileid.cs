using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fileid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_FileItem_fileId",
                table: "Contents");

            migrationBuilder.RenameColumn(
                name: "fileId",
                table: "Contents",
                newName: "FileId");

            migrationBuilder.RenameIndex(
                name: "IX_Contents_fileId",
                table: "Contents",
                newName: "IX_Contents_FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_FileItem_FileId",
                table: "Contents",
                column: "FileId",
                principalTable: "FileItem",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_FileItem_FileId",
                table: "Contents");

            migrationBuilder.RenameColumn(
                name: "FileId",
                table: "Contents",
                newName: "fileId");

            migrationBuilder.RenameIndex(
                name: "IX_Contents_FileId",
                table: "Contents",
                newName: "IX_Contents_fileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_FileItem_fileId",
                table: "Contents",
                column: "fileId",
                principalTable: "FileItem",
                principalColumn: "Id");
        }
    }
}
