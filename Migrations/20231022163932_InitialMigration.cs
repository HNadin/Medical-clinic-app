using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medical_clinic.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "Nurses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "Doctors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nurses_ServiceId",
                table: "Nurses",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_ServiceId",
                table: "Doctors",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Services_ServiceId",
                table: "Doctors",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Nurses_Services_ServiceId",
                table: "Nurses",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Services_ServiceId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Nurses_Services_ServiceId",
                table: "Nurses");

            migrationBuilder.DropIndex(
                name: "IX_Nurses_ServiceId",
                table: "Nurses");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_ServiceId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Nurses");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Doctors");
        }
    }
}
