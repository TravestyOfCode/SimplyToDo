using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyToDo.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSharedTasksAndLists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SharedToDoLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ToDoListId = table.Column<int>(type: "INTEGER", nullable: false),
                    SharedUserId = table.Column<string>(type: "TEXT", nullable: false),
                    CanComplete = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanEdit = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanDelete = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedToDoLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharedToDoLists_AspNetUsers_SharedUserId",
                        column: x => x.SharedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedToDoLists_ToDoTasks_ToDoListId",
                        column: x => x.ToDoListId,
                        principalTable: "ToDoTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SharedToDoTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ToDoTaskId = table.Column<int>(type: "INTEGER", nullable: false),
                    SharedUserId = table.Column<string>(type: "TEXT", nullable: false),
                    CanComplete = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanEdit = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanDelete = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedToDoTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharedToDoTasks_AspNetUsers_SharedUserId",
                        column: x => x.SharedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedToDoTasks_ToDoTasks_ToDoTaskId",
                        column: x => x.ToDoTaskId,
                        principalTable: "ToDoTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SharedToDoLists_SharedUserId",
                table: "SharedToDoLists",
                column: "SharedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedToDoLists_ToDoListId",
                table: "SharedToDoLists",
                column: "ToDoListId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedToDoTasks_SharedUserId",
                table: "SharedToDoTasks",
                column: "SharedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedToDoTasks_ToDoTaskId",
                table: "SharedToDoTasks",
                column: "ToDoTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SharedToDoLists");

            migrationBuilder.DropTable(
                name: "SharedToDoTasks");
        }
    }
}
