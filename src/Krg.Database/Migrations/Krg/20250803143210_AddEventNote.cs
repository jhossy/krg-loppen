using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Krg.Database.Migrations.Krg
{
    /// <inheritdoc />
    public partial class AddEventNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Event",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Event");
        }
    }
}
