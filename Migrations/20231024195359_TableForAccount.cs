using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medical_clinic.Migrations
{
    /// <inheritdoc />
    public partial class TableForAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ConfirmPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

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
    }
}
