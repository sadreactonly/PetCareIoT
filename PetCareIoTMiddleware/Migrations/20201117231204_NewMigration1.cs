using Microsoft.EntityFrameworkCore.Migrations;

namespace PetCareIoTMiddleware.Migrations
{
    public partial class NewMigration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Datetime",
                table: "TemperatureEvents",
                newName: "TimeStamp");

            migrationBuilder.RenameColumn(
                name: "Datetime",
                table: "PumpEvents",
                newName: "TimeStamp");

            migrationBuilder.RenameColumn(
                name: "Datetime",
                table: "LightEvents",
                newName: "TimeStamp");

            migrationBuilder.RenameColumn(
                name: "Datetime",
                table: "HumidityEvent",
                newName: "TimeStamp");

            migrationBuilder.RenameColumn(
                name: "Datetime",
                table: "FeederEvents",
                newName: "TimeStamp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "TemperatureEvents",
                newName: "Datetime");

            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "PumpEvents",
                newName: "Datetime");

            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "LightEvents",
                newName: "Datetime");

            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "HumidityEvent",
                newName: "Datetime");

            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "FeederEvents",
                newName: "Datetime");
        }
    }
}