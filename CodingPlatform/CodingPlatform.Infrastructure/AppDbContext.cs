using CodingPlatform.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodingPlatform.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<Challenge> Challenges { get; set; }
    public DbSet<Tip> Tips { get; set; }
    public DbSet<Submission> Submissions { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
}