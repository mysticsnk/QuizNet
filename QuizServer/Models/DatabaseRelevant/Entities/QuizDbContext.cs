using Microsoft.EntityFrameworkCore;
using QuizServer.Models.UserRelevant;

namespace QuizServer.Models.DatabaseRelevant.Entities;

public class QuizDbContext : DbContext
{
    public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options) { }

    public DbSet<UserAccount> UserAccounts => Set<UserAccount>();
}