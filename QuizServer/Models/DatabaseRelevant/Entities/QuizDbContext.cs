using Microsoft.EntityFrameworkCore;
using QuizServer.Models.Entities.QuizRelevant;
using QuizServer.Models.QuizRelevant.Abstracts;
using QuizServer.Models.QuizRelevant.Entities.Questions;
using QuizServer.Models.UserRelevant;

namespace QuizServer.Models.DatabaseRelevant.Entities;

public class QuizDbContext : DbContext
{
    public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options) { }

    public DbSet<UserAccount> UserAccounts => Set<UserAccount>();

    public DbSet<Quiz> Quizzes => Set<Quiz>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<SingleChoiceQuestion> SingleChoiceQuestions => Set<SingleChoiceQuestion>();
    public DbSet<MultiChoiceQuestion> MultipleChoiceQuestions => Set<MultiChoiceQuestion>();
    public DbSet<QuestionOption> QuestionOptions => Set<QuestionOption>();
}