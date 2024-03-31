using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medical_clinic.Migrations
{
    /// <inheritdoc />
    public partial class ChangeProvideServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProvideServices_Doctors_DoctorId",
                table: "ProvideServices");

            migrationBuilder.DropForeignKey(
                name: "FK_ProvideServices_Nurses_NurseId",
                table: "ProvideServices");

            migrationBuilder.AlterColumn<int>(
                name: "NurseId",
                table: "ProvideServices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "ProvideServices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ProvideServices_Doctors_DoctorId",
                table: "ProvideServices",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProvideServices_Nurses_NurseId",
                table: "ProvideServices",
                column: "NurseId",
                principalTable: "Nurses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProvideServices_Doctors_DoctorId",
                table: "ProvideServices");

            migrationBuilder.DropForeignKey(
                name: "FK_ProvideServices_Nurses_NurseId",
                table: "ProvideServices");

            migrationBuilder.AlterColumn<int>(
                name: "NurseId",
                table: "ProvideServices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "ProvideServices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProvideServices_Doctors_DoctorId",
                table: "ProvideServices",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProvideServices_Nurses_NurseId",
                table: "ProvideServices",
                column: "NurseId",
                principalTable: "Nurses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
