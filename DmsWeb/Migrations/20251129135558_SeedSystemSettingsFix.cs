using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DmsWeb.Migrations
{
    /// <inheritdoc />
    public partial class SeedSystemSettingsFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.InsertData(
                table: "SystemSettings",
                columns: new[] { "Id", "AllowedExtensions", "InstitutionName", "LogoPath", "MaxUploadSizeMb", "SystemName", "Theme" },
                values: new object[] { 1, ".pdf,.docx,.xlsx,.pptx", "Ankara Üniversitesi", null, 20, "Döküman Yönetim Sistemi", "dark" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SystemSettings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);
        }
    }
}
