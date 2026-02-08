using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiForAng.Migrations
{
    /// <inheritdoc />
    public partial class user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contect",
                table: "uses");

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "uses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "uses");

            migrationBuilder.AddColumn<string>(
                name: "Contect",
                table: "uses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
