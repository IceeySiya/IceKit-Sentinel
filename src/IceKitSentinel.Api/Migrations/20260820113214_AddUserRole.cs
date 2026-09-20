using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IceKitSentinel.Api.Migrations
{
    public partial class AddUserRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // The Role column already exists in the database.
            // This migration is intentionally left empty so that
            // EF Core can synchronize its migration history.
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Do not remove the existing Role column.
        }
    }
}