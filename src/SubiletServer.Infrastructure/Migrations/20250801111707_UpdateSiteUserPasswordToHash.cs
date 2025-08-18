using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubiletServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSiteUserPasswordToHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "SiteUsers");

            migrationBuilder.AddColumn<byte[]>(
                name: "Password_PasswordHash",
                table: "SiteUsers",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "Password_PasswordSalt",
                table: "SiteUsers",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password_PasswordHash",
                table: "SiteUsers");

            migrationBuilder.DropColumn(
                name: "Password_PasswordSalt",
                table: "SiteUsers");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "SiteUsers",
                type: "varchar(MAX)",
                nullable: false,
                defaultValue: "");
        }
    }
}
