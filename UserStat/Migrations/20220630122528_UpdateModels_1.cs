using Microsoft.EntityFrameworkCore.Migrations;

namespace UserStat.Migrations
{
    public partial class UpdateModels_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "startDate",
                table: "UserQueries",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "endDate",
                table: "UserQueries",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "UserQueries",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "count_sign_in",
                table: "QueryResult",
                newName: "Count_sign_in");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "QueryResult",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "user_id_id",
                table: "QueriesBridges",
                newName: "UserQueryId");

            migrationBuilder.RenameColumn(
                name: "query_id",
                table: "QueriesBridges",
                newName: "QueryId");

            migrationBuilder.RenameColumn(
                name: "queryGuid",
                table: "Queries",
                newName: "QueryGuid");

            migrationBuilder.RenameColumn(
                name: "percent",
                table: "Queries",
                newName: "Percent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "UserQueries",
                newName: "startDate");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "UserQueries",
                newName: "endDate");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserQueries",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "Count_sign_in",
                table: "QueryResult",
                newName: "count_sign_in");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "QueryResult",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UserQueryId",
                table: "QueriesBridges",
                newName: "user_id_id");

            migrationBuilder.RenameColumn(
                name: "QueryId",
                table: "QueriesBridges",
                newName: "query_id");

            migrationBuilder.RenameColumn(
                name: "QueryGuid",
                table: "Queries",
                newName: "queryGuid");

            migrationBuilder.RenameColumn(
                name: "Percent",
                table: "Queries",
                newName: "percent");
        }
    }
}
