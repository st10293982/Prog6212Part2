using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prog6212Part2.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewedFieldsToLecturerClaim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LecturerClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HourlyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HoursWorked = table.Column<double>(type: "float", nullable: false),
                    AdditionalNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DateSubmitted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    DocumentPath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReviewedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReviewedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LecturerClaims", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LecturerClaims");
        }
    }
}
