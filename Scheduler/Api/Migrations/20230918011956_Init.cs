using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scheduler.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    ScheduleId = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Floor = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => new { x.Number, x.ScheduleId });
                    table.ForeignKey(
                        name: "FK_Rooms_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoomNumber = table.Column<int>(type: "INTEGER", nullable: true),
                    RoomScheduleId = table.Column<int>(type: "INTEGER", nullable: true),
                    ScheduleId = table.Column<int>(type: "INTEGER", nullable: true),
                    CheckIn = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    CheckOut = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    FlightArrivalNumber = table.Column<string>(type: "TEXT", nullable: true),
                    FlightDepartureNumber = table.Column<string>(type: "TEXT", nullable: true),
                    FlightArrivalTime = table.Column<TimeOnly>(type: "TEXT", nullable: true),
                    FlightDepartureTime = table.Column<TimeOnly>(type: "TEXT", nullable: true),
                    BookingSource = table.Column<int>(type: "INTEGER", nullable: true),
                    Remarks = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Rooms_RoomNumber_ScheduleId",
                        columns: x => new { x.RoomNumber, x.ScheduleId },
                        principalTable: "Rooms",
                        principalColumns: new[] { "Number", "ScheduleId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReservationId = table.Column<int>(type: "INTEGER", nullable: true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    LastName = table.Column<string>(type: "TEXT", nullable: true),
                    Age = table.Column<int>(type: "INTEGER", nullable: true),
                    Note = table.Column<string>(type: "TEXT", nullable: true),
                    Prefix = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Person_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Person_ReservationId",
                table: "Person",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_RoomNumber_ScheduleId",
                table: "Reservations",
                columns: new[] { "RoomNumber", "ScheduleId" });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ScheduleId",
                table: "Reservations",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_ScheduleId",
                table: "Rooms",
                column: "ScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Schedules");
        }
    }
}
