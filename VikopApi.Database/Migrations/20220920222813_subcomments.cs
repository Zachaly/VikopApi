using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VikopApi.Database.Migrations
{
    public partial class subcomments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubComments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    MainCommentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubComments", x => new { x.CommentId, x.MainCommentId });
                    table.ForeignKey(
                        name: "FK_SubComments_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubComments_Comments_MainCommentId",
                        column: x => x.MainCommentId,
                        principalTable: "Comments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubComments_MainCommentId",
                table: "SubComments",
                column: "MainCommentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubComments");
        }
    }
}
