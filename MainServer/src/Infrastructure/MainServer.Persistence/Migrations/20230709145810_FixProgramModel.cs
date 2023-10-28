using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainServer.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixProgramModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasBeenActivated",
                table: "Programs");

            migrationBuilder.RenameColumn(
                name: "IsActived",
                table: "Programs",
                newName: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Programs",
                newName: "IsActived");

            migrationBuilder.AddColumn<bool>(
                name: "HasBeenActivated",
                table: "Programs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Programs",
                keyColumn: "Id",
                keyValue: 1,
                column: "HasBeenActivated",
                value: false);
        }
    }
}
