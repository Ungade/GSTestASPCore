using Microsoft.EntityFrameworkCore.Migrations;

namespace UserStat.Migrations
{
    public partial class UpdateDBContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QueryResult_Queries_QueryForeignKey",
                table: "QueryResult");

            migrationBuilder.RenameColumn(
                name: "QueryForeignKey",
                table: "QueryResult",
                newName: "QueryId");

            migrationBuilder.RenameIndex(
                name: "IX_QueryResult_QueryForeignKey",
                table: "QueryResult",
                newName: "IX_QueryResult_QueryId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Queries",
                newName: "QueryId");

            migrationBuilder.AddForeignKey(
                name: "FK_QueryResult_Queries_QueryId",
                table: "QueryResult",
                column: "QueryId",
                principalTable: "Queries",
                principalColumn: "QueryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QueryResult_Queries_QueryId",
                table: "QueryResult");

            migrationBuilder.RenameColumn(
                name: "QueryId",
                table: "QueryResult",
                newName: "QueryForeignKey");

            migrationBuilder.RenameIndex(
                name: "IX_QueryResult_QueryId",
                table: "QueryResult",
                newName: "IX_QueryResult_QueryForeignKey");

            migrationBuilder.RenameColumn(
                name: "QueryId",
                table: "Queries",
                newName: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_QueryResult_Queries_QueryForeignKey",
                table: "QueryResult",
                column: "QueryForeignKey",
                principalTable: "Queries",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
