using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace BIP.DataAccess.Migrations
{
    public partial class SeedRoles2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert 'User' role into the 'AspNetRoles' table if it does not already exist
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { Guid.NewGuid().ToString(), "User", "USER", Guid.NewGuid().ToString() }
            );

            // Insert 'Admin' role into the 'AspNetRoles' table if it does not already exist
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { Guid.NewGuid().ToString(), "Admin", "ADMIN", Guid.NewGuid().ToString() }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Clean up roles by deleting them in reverse order
            migrationBuilder.Sql("DELETE FROM [AspNetRoles] WHERE Name = 'User' OR Name = 'Admin'");
        }
    }
}
