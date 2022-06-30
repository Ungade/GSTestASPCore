using Microsoft.EntityFrameworkCore.Migrations;

namespace UserStat.Migrations
{
    public partial class UpdateModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "UserQueries",
                newName: "UserQueryId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "QueryResult",
                newName: "QueryResultId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "QueriesBridges",
                newName: "QueriesBridgeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserQueryId",
                table: "UserQueries",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "QueryResultId",
                table: "QueryResult",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "QueriesBridgeId",
                table: "QueriesBridges",
                newName: "id");
        }
    }
}
