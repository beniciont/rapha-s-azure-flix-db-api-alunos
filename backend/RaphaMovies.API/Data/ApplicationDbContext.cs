using Microsoft.EntityFrameworkCore;
using RaphaMovies.API.Models;

namespace RaphaMovies.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Rental> Rentals => Set<Rental>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
            
            entity.HasMany(u => u.Roles)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.Rentals)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Movie configuration
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasIndex(m => m.Genre);
            entity.HasIndex(m => m.Year);
            entity.HasIndex(m => m.Title);

            entity.Property(m => m.Rating)
                .HasColumnType("decimal(3,1)");

            entity.Property(m => m.RentalPrice)
                .HasColumnType("decimal(10,2)");
        });

        // UserRole configuration
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasIndex(r => new { r.UserId, r.Role }).IsUnique();
        });

        // Rental configuration
        modelBuilder.Entity<Rental>(entity =>
        {
            entity.HasIndex(r => r.Status);
            entity.HasIndex(r => r.DueDate);

            entity.HasOne(r => r.Movie)
                .WithMany(m => m.Rentals)
                .HasForeignKey(r => r.MovieId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Seed initial data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed admin user
        var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = adminId,
            Name = "Administrador",
            Email = "admin@raphamovies.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });

        modelBuilder.Entity<UserRole>().HasData(new UserRole
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            UserId = adminId,
            Role = "admin"
        });

        // Seed movies
        var movies = new[]
        {
            new Movie
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333001"),
                Title = "Oppenheimer",
                Synopsis = "A história do físico J. Robert Oppenheimer e seu papel no desenvolvimento da bomba atômica.",
                ImageUrl = "https://image.tmdb.org/t/p/w500/8Gxv8gSFCU0XGDykEGv7zR1n2ua.jpg",
                BackdropUrl = "https://image.tmdb.org/t/p/original/rLb2cwF3Pazuxaj0sRXQ037tGI1.jpg",
                TrailerUrl = "https://www.youtube.com/watch?v=uYPbbksJxIg",
                Year = 2023,
                Duration = "3h 1min",
                Rating = 8.9m,
                Genre = "Drama",
                RentalPrice = 14.99m,
                IsAvailable = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Movie
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333002"),
                Title = "Duna: Parte 2",
                Synopsis = "Paul Atreides se une aos Fremen enquanto busca vingança contra os conspiradores que destruíram sua família.",
                ImageUrl = "https://image.tmdb.org/t/p/w500/1pdfLvkbY9ohJlCjQH2CZjjYVvJ.jpg",
                BackdropUrl = "https://image.tmdb.org/t/p/original/xOMo8BRK7PfcJv9JCnx7s5hj0PX.jpg",
                TrailerUrl = "https://www.youtube.com/watch?v=Way9Dexny3w",
                Year = 2024,
                Duration = "2h 47min",
                Rating = 8.8m,
                Genre = "Ficção Científica",
                RentalPrice = 15.99m,
                IsAvailable = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Movie
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333003"),
                Title = "Pobres Criaturas",
                Synopsis = "A história de Bella Baxter, uma jovem trazida de volta à vida pelo brilhante cientista Dr. Godwin Baxter.",
                ImageUrl = "https://image.tmdb.org/t/p/w500/kCGlIMHnOm8JPXq3rXM6c5wMxcT.jpg",
                BackdropUrl = "https://image.tmdb.org/t/p/original/bQS43HSLZzMjZkcHJz4fGc7fNdz.jpg",
                TrailerUrl = "https://www.youtube.com/watch?v=RlbR5N6veqw",
                Year = 2023,
                Duration = "2h 21min",
                Rating = 8.4m,
                Genre = "Comédia",
                RentalPrice = 12.99m,
                IsAvailable = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Movie
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333004"),
                Title = "Assassinos da Lua das Flores",
                Synopsis = "Membros da tribo Osage nos EUA são assassinados sob circunstâncias misteriosas nos anos 1920.",
                ImageUrl = "https://image.tmdb.org/t/p/w500/dB6Krk806zeqd0YNp2ngQ9zXteH.jpg",
                BackdropUrl = "https://image.tmdb.org/t/p/original/1X7vow16X7CnCoexXh4H4F2yDJv.jpg",
                TrailerUrl = "https://www.youtube.com/watch?v=EP34Yoxs3FQ",
                Year = 2023,
                Duration = "3h 26min",
                Rating = 8.1m,
                Genre = "Crime",
                RentalPrice = 13.99m,
                IsAvailable = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Movie
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333005"),
                Title = "Barbie",
                Synopsis = "Barbie sofre uma crise existencial e parte em uma jornada de autodescoberta no mundo real.",
                ImageUrl = "https://image.tmdb.org/t/p/w500/iuFNMS8U5cb6xfzi51Dbkovj7vM.jpg",
                BackdropUrl = "https://image.tmdb.org/t/p/original/nHf61UzkfFno5X1ofIhugCPus2R.jpg",
                TrailerUrl = "https://www.youtube.com/watch?v=pBk4NYhWNMM",
                Year = 2023,
                Duration = "1h 54min",
                Rating = 7.8m,
                Genre = "Comédia",
                RentalPrice = 11.99m,
                IsAvailable = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Movie
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333006"),
                Title = "Anatomia de uma Queda",
                Synopsis = "Uma escritora é suspeita de assassinar seu marido quando ele é encontrado morto.",
                ImageUrl = "https://image.tmdb.org/t/p/w500/kQs6keheMwCxJxrzV83VUwFtHkB.jpg",
                BackdropUrl = "https://image.tmdb.org/t/p/original/9jPoyxjiEYPylUIMI3Ntixf8z3M.jpg",
                TrailerUrl = "https://www.youtube.com/watch?v=MJlpBrQKCBc",
                Year = 2023,
                Duration = "2h 32min",
                Rating = 8.2m,
                Genre = "Suspense",
                RentalPrice = 12.99m,
                IsAvailable = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Movie
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333007"),
                Title = "Homem-Aranha: Através do Aranhaverso",
                Synopsis = "Miles Morales atravessa o Multiverso e encontra uma equipe de Homens-Aranha.",
                ImageUrl = "https://image.tmdb.org/t/p/w500/8Vt6mWEReuy4Of61Lnj5Xj704m8.jpg",
                BackdropUrl = "https://image.tmdb.org/t/p/original/4HodYYKEIsGOdinkGi2Ucz6X9i0.jpg",
                TrailerUrl = "https://www.youtube.com/watch?v=shW9i6k8cB0",
                Year = 2023,
                Duration = "2h 20min",
                Rating = 8.7m,
                Genre = "Animação",
                RentalPrice = 14.99m,
                IsAvailable = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Movie
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333008"),
                Title = "O Menu",
                Synopsis = "Um casal viaja para uma ilha remota para comer em um restaurante exclusivo com um chef famoso.",
                ImageUrl = "https://image.tmdb.org/t/p/w500/v31MsWhF9WFh7Qo09IvbZOFQ03y.jpg",
                BackdropUrl = "https://image.tmdb.org/t/p/original/64e7jXfNRaFrPCnWjSHdNxBmaAA.jpg",
                TrailerUrl = "https://www.youtube.com/watch?v=C_uTkUGcHv4",
                Year = 2022,
                Duration = "1h 47min",
                Rating = 7.5m,
                Genre = "Terror",
                RentalPrice = 10.99m,
                IsAvailable = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Movie
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333009"),
                Title = "Top Gun: Maverick",
                Synopsis = "Depois de 30 anos de serviço, Pete Mitchell é convocado para treinar um grupo de pilotos de elite.",
                ImageUrl = "https://image.tmdb.org/t/p/w500/62HCnUTziyWcpDaBO2i1DX17ljH.jpg",
                BackdropUrl = "https://image.tmdb.org/t/p/original/AaV1YIdWKhxX5V4FlTxU7S2QXQL.jpg",
                TrailerUrl = "https://www.youtube.com/watch?v=qSqVVswa420",
                Year = 2022,
                Duration = "2h 11min",
                Rating = 8.6m,
                Genre = "Ação",
                RentalPrice = 12.99m,
                IsAvailable = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Movie
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333010"),
                Title = "Tudo em Todo o Lugar ao Mesmo Tempo",
                Synopsis = "Uma imigrante chinesa descobre que precisa se conectar com versões dela mesma em universos paralelos.",
                ImageUrl = "https://image.tmdb.org/t/p/w500/w3LxiVYdWWRvEVdn5RYq6jIqkb1.jpg",
                BackdropUrl = "https://image.tmdb.org/t/p/original/fOy2Jurz9k6RnJnMUMRDAgBwru2.jpg",
                TrailerUrl = "https://www.youtube.com/watch?v=wxN1T1uxQ2g",
                Year = 2022,
                Duration = "2h 19min",
                Rating = 8.7m,
                Genre = "Ficção Científica",
                RentalPrice = 13.99m,
                IsAvailable = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        };

        modelBuilder.Entity<Movie>().HasData(movies);
    }
}
