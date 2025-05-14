using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Registrations_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_EventId",
                table: "Registrations",
                column: "EventId");

            // Seed data
            var event1Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var event2Id = Guid.Parse("22222222-2222-2222-2222-222222222222");

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Title", "Description", "Date", "Location", "Category", "Capacity", "Status" },
                values: new object[,]
                {
                        { event1Id, "Tech Conference", "A conference about the latest in tech.", new DateTime(2025, 6, 1, 9, 0, 0, DateTimeKind.Utc), "Main Hall", "Technology", 100, 1 },
                        { event2Id, "Art Expo", "An exhibition of modern art.", new DateTime(2025, 7, 15, 10, 0, 0, DateTimeKind.Utc), "Gallery Room", "Art", 50, 1 }
                }
            );

            migrationBuilder.InsertData(
                table: "Registrations",
                columns: new[] { "Id", "EventId", "UserId", "RegisteredAt" },
                values: new object[,]
                {
                        { Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), event1Id, "user1", new DateTime(2025, 5, 15, 8, 0, 0, DateTimeKind.Utc) },
                        { Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), event1Id, "user2", new DateTime(2025, 5, 16, 9, 30, 0, DateTimeKind.Utc) },
                        { Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), event1Id, "user3", new DateTime(2025, 5, 17, 10, 45, 0, DateTimeKind.Utc) }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
