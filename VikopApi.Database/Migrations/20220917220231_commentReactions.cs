using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VikopApi.Database.Migrations
{
    public partial class commentReactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentsReactions_AspNetUsers_UserId",
                table: "CommentsReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentsReactions_Comments_CommentId",
                table: "CommentsReactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentsReactions",
                table: "CommentsReactions");

            migrationBuilder.RenameTable(
                name: "CommentsReactions",
                newName: "CommentReactions");

            migrationBuilder.RenameIndex(
                name: "IX_CommentsReactions_UserId",
                table: "CommentReactions",
                newName: "IX_CommentReactions_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentReactions",
                table: "CommentReactions",
                columns: new[] { "CommentId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReactions_AspNetUsers_UserId",
                table: "CommentReactions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReactions_Comments_CommentId",
                table: "CommentReactions",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentReactions_AspNetUsers_UserId",
                table: "CommentReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentReactions_Comments_CommentId",
                table: "CommentReactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentReactions",
                table: "CommentReactions");

            migrationBuilder.RenameTable(
                name: "CommentReactions",
                newName: "CommentsReactions");

            migrationBuilder.RenameIndex(
                name: "IX_CommentReactions_UserId",
                table: "CommentsReactions",
                newName: "IX_CommentsReactions_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentsReactions",
                table: "CommentsReactions",
                columns: new[] { "CommentId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommentsReactions_AspNetUsers_UserId",
                table: "CommentsReactions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentsReactions_Comments_CommentId",
                table: "CommentsReactions",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }
    }
}
