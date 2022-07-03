using Microsoft.EntityFrameworkCore.Migrations;

namespace UserStat.Migrations
{
    public partial class QueryToQueryBridgeOneToOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_QueriesBridges_QueryId",
                table: "QueriesBridges",
                column: "QueryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QueriesBridges_Queries_QueryId",
                table: "QueriesBridges",
                column: "QueryId",
                principalTable: "Queries",
                principalColumn: "QueryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QueriesBridges_Queries_QueryId",
                table: "QueriesBridges");

            migrationBuilder.DropIndex(
                name: "IX_QueriesBridges_QueryId",
                table: "QueriesBridges");
        }
    }
}
