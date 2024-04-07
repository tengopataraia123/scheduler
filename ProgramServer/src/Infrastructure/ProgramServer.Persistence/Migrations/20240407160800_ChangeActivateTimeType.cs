using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgramServer.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeActivateTimeType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Subjects_SubjectId",
                table: "Events");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActivationTime",
                table: "BluetoothCodes",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Subjects_SubjectId",
                table: "Events",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Subjects_SubjectId",
                table: "Events");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActivationTime",
                table: "BluetoothCodes",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Subjects_SubjectId",
                table: "Events",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
