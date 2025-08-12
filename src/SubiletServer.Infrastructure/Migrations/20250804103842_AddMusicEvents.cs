using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubiletServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMusicEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MusicEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistName = table.Column<string>(type: "varchar(MAX)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "varchar(MAX)", maxLength: 1000, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "varchar(MAX)", maxLength: 300, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "varchar(MAX)", maxLength: 500, nullable: false),
                    Genre = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicEvents", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MusicEvents_Date",
                table: "MusicEvents",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_MusicEvents_Genre",
                table: "MusicEvents",
                column: "Genre");

            migrationBuilder.CreateIndex(
                name: "IX_MusicEvents_Status",
                table: "MusicEvents",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MusicEvents");
        }
    }
}
