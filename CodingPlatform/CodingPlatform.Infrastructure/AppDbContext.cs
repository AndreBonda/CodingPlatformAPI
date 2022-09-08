using CodingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodingPlatform.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<UserTournamentParticipations> UserTournamentParticipations { get; set; }
    public DbSet<CurrentChallenge> CurrentChallenges { get; set; }
    public DbSet<Tip> Tips { get; set; }
}