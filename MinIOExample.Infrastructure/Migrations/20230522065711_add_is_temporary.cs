using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinIOExample.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_is_temporary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTemporary",
                table: "FileMetadata",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTemporary",
                table: "FileMetadata");
        }
    }
}
