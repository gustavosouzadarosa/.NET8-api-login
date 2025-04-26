using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiLogin.Migrations
{
    /// <inheritdoc />
    public partial class _20250426 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CampoAdicional2",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CampoAdicional2",
                table: "AspNetUsers");
        }
    }
}
