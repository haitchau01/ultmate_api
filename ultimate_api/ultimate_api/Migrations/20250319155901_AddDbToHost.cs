using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ultimate_api.Migrations
{
    /// <inheritdoc />
    public partial class AddDbToHost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Address = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Country = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    Department = table.Column<string>(type: "text", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "Address", "Country", "Name" },
                values: new object[,]
                {
                    { new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"), "312 Forest Avenue, BF 923", "VIETNAM", "Admin_Solutions Ltd" },
                    { new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "583 Wall Dr. Gwynn Oak, MD 21207", "VIETNAM", "IT_Solutions Ltd" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "CompanyId", "Country", "DateOfBirth", "Department", "Email", "FirstName", "LastName", "PhoneNumber", "UserId" },
                values: new object[,]
                {
                    { new Guid("80abbca8-664d-4b20-b5de-024705497d4a"), "HO CHI MINH CITY", new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "VIET NAME", new DateTime(2003, 3, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Sam", "Raiden", "0347347343", null },
                    { new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"), "QUANG NINH CITY", new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "VIET NAME", new DateTime(2002, 3, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Deo", "Rita", "0938466233", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyId",
                table: "Users",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId",
                table: "Users",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
