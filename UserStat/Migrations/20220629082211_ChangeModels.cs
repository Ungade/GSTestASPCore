using Microsoft.EntityFrameworkCore.Migrations;

namespace UserStat.Migrations
{
    public partial class ChangeModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserQueries",
                table: "UserQueries");

            migrationBuilder.RenameColumn(
                name: "count_sign_in",
                table: "UserQueries",
                newName: "id");

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "UserQueries",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "UserQueries",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserQueries",
                table: "UserQueries",
                column: "id");

            migrationBuilder.CreateTable(
                name: "QueriesBridges",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_id_id = table.Column<long>(type: "INTEGER", nullable: false),
                    query_id = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueriesBridges", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "QueryResult",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueryResult", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Queries",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    queryGuid = table.Column<string>(type: "TEXT", nullable: true),
                    percent = table.Column<ushort>(type: "INTEGER", nullable: false),
                    resultid = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Queries", x => x.id);
                    table.ForeignKey(
                        name: "FK_Queries_QueryResult_resultid",
                        column: x => x.resultid,
                        principalTable: "QueryResult",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Queries_resultid",
                table: "Queries",
                column: "resultid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Queries");

            migrationBuilder.DropTable(
                name: "QueriesBridges");

            migrationBuilder.DropTable(
                name: "QueryResult");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserQueries",
                table: "UserQueries");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "UserQueries",
                newName: "count_sign_in");

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                table: "UserQueries",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "count_sign_in",
                table: "UserQueries",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserQueries",
                table: "UserQueries",
                column: "user_id");
        }
    }
}
