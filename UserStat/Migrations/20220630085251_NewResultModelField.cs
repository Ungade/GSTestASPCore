using Microsoft.EntityFrameworkCore.Migrations;

namespace UserStat.Migrations
{
    public partial class NewResultModelField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Queries_QueryResult_resultid",
                table: "Queries");

            migrationBuilder.DropIndex(
                name: "IX_Queries_resultid",
                table: "Queries");

            migrationBuilder.DropColumn(
                name: "resultid",
                table: "Queries");

            migrationBuilder.AddColumn<long>(
                name: "QueryForeignKey",
                table: "QueryResult",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_QueryResult_QueryForeignKey",
                table: "QueryResult",
                column: "QueryForeignKey",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QueryResult_Queries_QueryForeignKey",
                table: "QueryResult",
                column: "QueryForeignKey",
                principalTable: "Queries",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QueryResult_Queries_QueryForeignKey",
                table: "QueryResult");

            migrationBuilder.DropIndex(
                name: "IX_QueryResult_QueryForeignKey",
                table: "QueryResult");

            migrationBuilder.DropColumn(
                name: "QueryForeignKey",
                table: "QueryResult");

            migrationBuilder.AddColumn<long>(
                name: "resultid",
                table: "Queries",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Queries_resultid",
                table: "Queries",
                column: "resultid");

            migrationBuilder.AddForeignKey(
                name: "FK_Queries_QueryResult_resultid",
                table: "Queries",
                column: "resultid",
                principalTable: "QueryResult",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
