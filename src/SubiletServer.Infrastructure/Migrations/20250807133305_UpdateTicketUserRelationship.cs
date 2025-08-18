using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubiletServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTicketUserRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_SiteUsers_UserId",
                table: "Tickets");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_UserId",
                table: "Tickets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_UserId",
                table: "Tickets");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_SiteUsers_UserId",
                table: "Tickets",
                column: "UserId",
                principalTable: "SiteUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
