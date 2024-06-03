using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IventisEventApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Genre = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Venues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    BoundingBox = table.Column<string>(type: "TEXT", nullable: false),
                    Capacity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    VenueId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventsArtists",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ArtistId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventsArtists", x => new { x.EventId, x.ArtistId });
                    table.ForeignKey(
                        name: "FK_EventsArtists_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventsArtists_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Artists",
                columns: new[] { "Id", "Genre", "Name" },
                values: new object[,]
                {
                    { new Guid("87f7477c-e1e9-4d90-92c7-c577dc50cc9f"), "Pop", "John Doe" },
                    { new Guid("95944c32-33e6-4d1a-abe2-0287500383e3"), "Country", "Jane Doe" }
                });

            migrationBuilder.InsertData(
                table: "Venues",
                columns: new[] { "Id", "BoundingBox", "Capacity", "Name" },
                values: new object[,]
                {
                    { new Guid("3e454d63-80db-4dfa-a6fc-8af2a078e794"), "53.228317#-0.546745#53.228295#-0.548888", 100, "Venue 1" },
                    { new Guid("d7ca61fb-babe-4819-8ef2-82f66b18fc50"), "52.9125#-1.183384#52.909932#-1.186041", 50, "Venue 2" }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Date", "Name", "VenueId" },
                values: new object[,]
                {
                    { new Guid("322ec951-1d88-4063-8920-2fd5831aa2c8"), new DateOnly(2025, 6, 4), "Event 2", new Guid("d7ca61fb-babe-4819-8ef2-82f66b18fc50") },
                    { new Guid("3da5127b-06ee-4503-8c26-55b43bde59d2"), new DateOnly(2024, 6, 4), "Event 1", new Guid("3e454d63-80db-4dfa-a6fc-8af2a078e794") },
                    { new Guid("c6eb77ee-bb53-4bb3-9b70-a5e82030501d"), new DateOnly(2024, 6, 7), "Event 3", new Guid("3e454d63-80db-4dfa-a6fc-8af2a078e794") }
                });

            migrationBuilder.InsertData(
                table: "EventsArtists",
                columns: new[] { "ArtistId", "EventId" },
                values: new object[,]
                {
                    { new Guid("95944c32-33e6-4d1a-abe2-0287500383e3"), new Guid("322ec951-1d88-4063-8920-2fd5831aa2c8") },
                    { new Guid("87f7477c-e1e9-4d90-92c7-c577dc50cc9f"), new Guid("3da5127b-06ee-4503-8c26-55b43bde59d2") },
                    { new Guid("95944c32-33e6-4d1a-abe2-0287500383e3"), new Guid("3da5127b-06ee-4503-8c26-55b43bde59d2") },
                    { new Guid("87f7477c-e1e9-4d90-92c7-c577dc50cc9f"), new Guid("c6eb77ee-bb53-4bb3-9b70-a5e82030501d") },
                    { new Guid("95944c32-33e6-4d1a-abe2-0287500383e3"), new Guid("c6eb77ee-bb53-4bb3-9b70-a5e82030501d") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_VenueId",
                table: "Events",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_EventsArtists_ArtistId",
                table: "EventsArtists",
                column: "ArtistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventsArtists");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Venues");
        }
    }
}
