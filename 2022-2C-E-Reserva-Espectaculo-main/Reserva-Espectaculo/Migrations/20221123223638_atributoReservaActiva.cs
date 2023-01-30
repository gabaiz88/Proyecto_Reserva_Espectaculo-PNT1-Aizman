using Microsoft.EntityFrameworkCore.Migrations;

namespace Reserva_Espectaculo.Migrations
{
    public partial class atributoReservaActiva : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activa",
                table: "Reservas",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activa",
                table: "Reservas");
        }
    }
}
