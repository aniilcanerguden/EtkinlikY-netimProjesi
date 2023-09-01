using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EtkinlikYönetimProjesi.Migrations
{
    /// <inheritdoc />
    public partial class addcategoryName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Events");
        }
    }
}
