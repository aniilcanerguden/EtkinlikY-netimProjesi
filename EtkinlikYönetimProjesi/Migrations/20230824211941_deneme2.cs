using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EtkinlikYönetimProjesi.Migrations
{
    /// <inheritdoc />
    public partial class deneme2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "Email", "FirstName", "LastName", "Password", "PasswordRepeat", "UserRole" },
                values: new object[] { 12, "admin@gmail.com", "Marty", "McFly", "admin1234", "admin1234", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 12);
        }
    }
}
