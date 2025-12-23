using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RaphaMovies.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Synopsis = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BackdropUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TrailerUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(3,1)", nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RentalPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rentals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RentedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rentals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rentals_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rentals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "BackdropUrl", "CreatedAt", "Duration", "Genre", "ImageUrl", "IsAvailable", "Rating", "RentalPrice", "Synopsis", "Title", "TrailerUrl", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333001"), "https://image.tmdb.org/t/p/original/rLb2cwF3Pazuxaj0sRXQ037tGI1.jpg", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "3h 1min", "Drama", "https://image.tmdb.org/t/p/w500/8Gxv8gSFCU0XGDykEGv7zR1n2ua.jpg", true, 8.9m, 14.99m, "A história do físico J. Robert Oppenheimer e seu papel no desenvolvimento da bomba atômica.", "Oppenheimer", "https://www.youtube.com/watch?v=uYPbbksJxIg", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2023 },
                    { new Guid("33333333-3333-3333-3333-333333333002"), "https://image.tmdb.org/t/p/original/xOMo8BRK7PfcJv9JCnx7s5hj0PX.jpg", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "2h 47min", "Ficção Científica", "https://image.tmdb.org/t/p/w500/1pdfLvkbY9ohJlCjQH2CZjjYVvJ.jpg", true, 8.8m, 15.99m, "Paul Atreides se une aos Fremen enquanto busca vingança contra os conspiradores que destruíram sua família.", "Duna: Parte 2", "https://www.youtube.com/watch?v=Way9Dexny3w", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2024 },
                    { new Guid("33333333-3333-3333-3333-333333333003"), "https://image.tmdb.org/t/p/original/bQS43HSLZzMjZkcHJz4fGc7fNdz.jpg", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "2h 21min", "Comédia", "https://image.tmdb.org/t/p/w500/kCGlIMHnOm8JPXq3rXM6c5wMxcT.jpg", true, 8.4m, 12.99m, "A história de Bella Baxter, uma jovem trazida de volta à vida pelo brilhante cientista Dr. Godwin Baxter.", "Pobres Criaturas", "https://www.youtube.com/watch?v=RlbR5N6veqw", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2023 },
                    { new Guid("33333333-3333-3333-3333-333333333004"), "https://image.tmdb.org/t/p/original/1X7vow16X7CnCoexXh4H4F2yDJv.jpg", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "3h 26min", "Crime", "https://image.tmdb.org/t/p/w500/dB6Krk806zeqd0YNp2ngQ9zXteH.jpg", true, 8.1m, 13.99m, "Membros da tribo Osage nos EUA são assassinados sob circunstâncias misteriosas nos anos 1920.", "Assassinos da Lua das Flores", "https://www.youtube.com/watch?v=EP34Yoxs3FQ", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2023 },
                    { new Guid("33333333-3333-3333-3333-333333333005"), "https://image.tmdb.org/t/p/original/nHf61UzkfFno5X1ofIhugCPus2R.jpg", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "1h 54min", "Comédia", "https://image.tmdb.org/t/p/w500/iuFNMS8U5cb6xfzi51Dbkovj7vM.jpg", true, 7.8m, 11.99m, "Barbie sofre uma crise existencial e parte em uma jornada de autodescoberta no mundo real.", "Barbie", "https://www.youtube.com/watch?v=pBk4NYhWNMM", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2023 },
                    { new Guid("33333333-3333-3333-3333-333333333006"), "https://image.tmdb.org/t/p/original/9jPoyxjiEYPylUIMI3Ntixf8z3M.jpg", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "2h 32min", "Suspense", "https://image.tmdb.org/t/p/w500/kQs6keheMwCxJxrzV83VUwFtHkB.jpg", true, 8.2m, 12.99m, "Uma escritora é suspeita de assassinar seu marido quando ele é encontrado morto.", "Anatomia de uma Queda", "https://www.youtube.com/watch?v=MJlpBrQKCBc", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2023 },
                    { new Guid("33333333-3333-3333-3333-333333333007"), "https://image.tmdb.org/t/p/original/4HodYYKEIsGOdinkGi2Ucz6X9i0.jpg", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "2h 20min", "Animação", "https://image.tmdb.org/t/p/w500/8Vt6mWEReuy4Of61Lnj5Xj704m8.jpg", true, 8.7m, 14.99m, "Miles Morales atravessa o Multiverso e encontra uma equipe de Homens-Aranha.", "Homem-Aranha: Através do Aranhaverso", "https://www.youtube.com/watch?v=shW9i6k8cB0", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2023 },
                    { new Guid("33333333-3333-3333-3333-333333333008"), "https://image.tmdb.org/t/p/original/64e7jXfNRaFrPCnWjSHdNxBmaAA.jpg", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "1h 47min", "Terror", "https://image.tmdb.org/t/p/w500/v31MsWhF9WFh7Qo09IvbZOFQ03y.jpg", true, 7.5m, 10.99m, "Um casal viaja para uma ilha remota para comer em um restaurante exclusivo com um chef famoso.", "O Menu", "https://www.youtube.com/watch?v=C_uTkUGcHv4", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2022 },
                    { new Guid("33333333-3333-3333-3333-333333333009"), "https://image.tmdb.org/t/p/original/AaV1YIdWKhxX5V4FlTxU7S2QXQL.jpg", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "2h 11min", "Ação", "https://image.tmdb.org/t/p/w500/62HCnUTziyWcpDaBO2i1DX17ljH.jpg", true, 8.6m, 12.99m, "Depois de 30 anos de serviço, Pete Mitchell é convocado para treinar um grupo de pilotos de elite.", "Top Gun: Maverick", "https://www.youtube.com/watch?v=qSqVVswa420", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2022 },
                    { new Guid("33333333-3333-3333-3333-333333333010"), "https://image.tmdb.org/t/p/original/fOy2Jurz9k6RnJnMUMRDAgBwru2.jpg", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "2h 19min", "Ficção Científica", "https://image.tmdb.org/t/p/w500/w3LxiVYdWWRvEVdn5RYq6jIqkb1.jpg", true, 8.7m, 13.99m, "Uma imigrante chinesa descobre que precisa se conectar com versões dela mesma em universos paralelos.", "Tudo em Todo o Lugar ao Mesmo Tempo", "https://www.youtube.com/watch?v=wxN1T1uxQ2g", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2022 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "Name", "PasswordHash", "UpdatedAt" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@raphamovies.com", "Administrador", "$2a$11$3FvDwFsOIiw4bhhagkEaTuYrjpaWCSQmdMt6bPxYlqNlNUv7SFVYa", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "Role", "UserId" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), "admin", new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Genre",
                table: "Movies",
                column: "Genre");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Title",
                table: "Movies",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Year",
                table: "Movies",
                column: "Year");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_DueDate",
                table: "Rentals",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_MovieId",
                table: "Rentals",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_Status",
                table: "Rentals",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_UserId",
                table: "Rentals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId_Role",
                table: "UserRoles",
                columns: new[] { "UserId", "Role" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rentals");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
