using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Group5Flights.Migrations
{
    /// <inheritdoc />
    public partial class Phase4Refactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airlines",
                columns: table => new
                {
                    AirlineId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ImageName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airlines", x => x.AirlineId);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    FlightId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightCode = table.Column<string>(type: "TEXT", nullable: false),
                    From = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    To = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CabinType = table.Column<string>(type: "TEXT", nullable: false),
                    Emission = table.Column<string>(type: "TEXT", nullable: false),
                    AircraftType = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    AirlineId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.FlightId);
                    table.ForeignKey(
                        name: "FK_Flights_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "AirlineId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReservedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK_Reservations_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "FlightId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Airlines",
                columns: new[] { "AirlineId", "ImageName", "Name" },
                values: new object[,]
                {
                    { 1, "delta.png", "Delta Airlines" },
                    { 2, "united.png", "United Airlines" },
                    { 3, "american.png", "American Airlines" }
                });

            migrationBuilder.InsertData(
                table: "Flights",
                columns: new[] { "FlightId", "AircraftType", "AirlineId", "ArrivalTime", "CabinType", "Date", "DepartureTime", "Emission", "FlightCode", "From", "Price", "To" },
                values: new object[,]
                {
                    { 1, "Airbus 320 Family", 1, new DateTime(2026, 4, 27, 10, 30, 0, 0, DateTimeKind.Local), "Economy", new DateTime(2026, 4, 27, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 4, 27, 8, 0, 0, 0, DateTimeKind.Local), "Low", "DL1001", "New York", 199.99m, "Chicago" },
                    { 2, "Boeing 737 Family", 1, new DateTime(2026, 4, 28, 13, 0, 0, 0, DateTimeKind.Local), "Business", new DateTime(2026, 4, 28, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 4, 28, 10, 0, 0, 0, DateTimeKind.Local), "Medium", "DL2002", "Chicago", 499.99m, "Los Angeles" },
                    { 3, "Airbus 320 Family", 2, new DateTime(2026, 4, 27, 12, 0, 0, 0, DateTimeKind.Local), "Economy Plus", new DateTime(2026, 4, 27, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 4, 27, 9, 0, 0, 0, DateTimeKind.Local), "Low", "UA101", "New York", 249.99m, "Miami" },
                    { 4, "Boeing 737 Family", 2, new DateTime(2026, 4, 29, 9, 30, 0, 0, DateTimeKind.Local), "Basic Economy", new DateTime(2026, 4, 29, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 4, 29, 7, 0, 0, 0, DateTimeKind.Local), "High", "UA202", "Chicago", 149.99m, "Dallas" },
                    { 5, "Airbus 320 Family", 3, new DateTime(2026, 4, 27, 16, 30, 0, 0, DateTimeKind.Local), "Economy", new DateTime(2026, 4, 27, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 4, 27, 14, 0, 0, 0, DateTimeKind.Local), "Medium", "AA303", "Los Angeles", 179.99m, "Seattle" },
                    { 6, "Boeing 737 Family", 3, new DateTime(2026, 4, 28, 9, 0, 0, 0, DateTimeKind.Local), "Business", new DateTime(2026, 4, 28, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 4, 28, 6, 0, 0, 0, DateTimeKind.Local), "Low", "AA404", "New York", 399.99m, "Miami" },
                    { 7, "Airbus 320 Family", 1, new DateTime(2026, 4, 29, 16, 0, 0, 0, DateTimeKind.Local), "Economy", new DateTime(2026, 4, 29, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 4, 29, 12, 0, 0, 0, DateTimeKind.Local), "Medium", "DL3003", "Seattle", 299.99m, "Chicago" },
                    { 8, "Airbus 320 Family", 2, new DateTime(2026, 4, 27, 17, 30, 0, 0, DateTimeKind.Local), "Economy Plus", new DateTime(2026, 4, 27, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 4, 27, 15, 0, 0, 0, DateTimeKind.Local), "Low", "UA505", "Dallas", 229.99m, "Los Angeles" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AirlineId",
                table: "Flights",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_FlightId",
                table: "Reservations",
                column: "FlightId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Airlines");
        }
    }
}
