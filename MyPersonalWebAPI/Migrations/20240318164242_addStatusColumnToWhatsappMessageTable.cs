using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyPersonalWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class addStatusColumnToWhatsappMessageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "whatsAppMessages",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "whatsAppMessages");
        }
    }
}
