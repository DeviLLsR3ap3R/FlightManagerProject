using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightManager.Migrations
{
    /// <inheritdoc />
    public partial class AddField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FlightId",
                table: "Reservations",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlightId",
                table: "Reservations");
        }
    }
}
