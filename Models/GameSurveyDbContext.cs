using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoreMvc.Models
{
    public class GameSurveyDbContext : IdentityDbContext
    {
        public GameSurveyDbContext(DbContextOptions<GameSurveyDbContext> options)
            : base(options)
        {
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<SurveyResponse> SurveyResponses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Required for Identity

            modelBuilder.Entity<Genre>()
                .HasIndex(g => g.Name)
                .IsUnique();

            modelBuilder.Entity<SurveyResponse>()
                .HasOne(sr => sr.Genre)
                .WithMany()
                .HasForeignKey(sr => sr.GenreId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed Genres
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Action", Description = "Fast-paced, combat-focused games" },
                new Genre { Id = 2, Name = "RPG", Description = "Role-playing games with deep stories" },
                new Genre { Id = 3, Name = "Strategy", Description = "Games requiring planning and tactics" },
                new Genre { Id = 4, Name = "Simulation", Description = "Games simulating real-world activities" },
                new Genre { Id = 5, Name = "Sports", Description = "Games based on athletic competitions" },
                new Genre { Id = 6, Name = "Puzzle", Description = "Games focused on problem-solving" }
            );
        }
    }
}