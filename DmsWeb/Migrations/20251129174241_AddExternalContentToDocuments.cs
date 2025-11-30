using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DmsWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddExternalContentToDocuments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalContent",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalContent",
                table: "Documents");
        }
    }
}
