using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VikopApi.Database.Migrations
{
    public partial class namechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FindingReports_AspNetUsers_UserId",
                table: "FindingReports");

            migrationBuilder.DropForeignKey(
                name: "FK_PostReports_AspNetUsers_UserId",
                table: "PostReports");

            migrationBuilder.DropIndex(
                name: "IX_PostReports_UserId",
                table: "PostReports");

            migrationBuilder.DropIndex(
                name: "IX_FindingReports_UserId",
                table: "FindingReports");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PostReports");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FindingReports");

            migrationBuilder.AlterColumn<string>(
                name: "ReportingUserId",
                table: "PostReports",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ReportingUserId",
                table: "FindingReports",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_PostReports_ReportingUserId",
                table: "PostReports",
                column: "ReportingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingReports_ReportingUserId",
                table: "FindingReports",
                column: "ReportingUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FindingReports_AspNetUsers_ReportingUserId",
                table: "FindingReports",
                column: "ReportingUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostReports_AspNetUsers_ReportingUserId",
                table: "PostReports",
                column: "ReportingUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FindingReports_AspNetUsers_ReportingUserId",
                table: "FindingReports");

            migrationBuilder.DropForeignKey(
                name: "FK_PostReports_AspNetUsers_ReportingUserId",
                table: "PostReports");

            migrationBuilder.DropIndex(
                name: "IX_PostReports_ReportingUserId",
                table: "PostReports");

            migrationBuilder.DropIndex(
                name: "IX_FindingReports_ReportingUserId",
                table: "FindingReports");

            migrationBuilder.AlterColumn<string>(
                name: "ReportingUserId",
                table: "PostReports",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PostReports",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReportingUserId",
                table: "FindingReports",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "FindingReports",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostReports_UserId",
                table: "PostReports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingReports_UserId",
                table: "FindingReports",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FindingReports_AspNetUsers_UserId",
                table: "FindingReports",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostReports_AspNetUsers_UserId",
                table: "PostReports",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
