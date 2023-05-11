using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Connectio.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserMentions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostUserMention",
                columns: table => new
                {
                    PostMentionsId = table.Column<int>(type: "int", nullable: false),
                    UserMentionsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostUserMention", x => new { x.PostMentionsId, x.UserMentionsId });
                    table.ForeignKey(
                        name: "FK_PostUserMention_AspNetUsers_UserMentionsId",
                        column: x => x.UserMentionsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostUserMention_Posts_PostMentionsId",
                        column: x => x.PostMentionsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostUserMention_UserMentionsId",
                table: "PostUserMention",
                column: "UserMentionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostUserMention");
        }
    }
}
