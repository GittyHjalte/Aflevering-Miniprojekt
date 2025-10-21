using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveButtons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownvoteButton",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "UpvoteButton",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "DownvoteButton",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UpvoteButton",
                table: "Comments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DownvoteButton",
                table: "Posts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UpvoteButton",
                table: "Posts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DownvoteButton",
                table: "Comments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UpvoteButton",
                table: "Comments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
