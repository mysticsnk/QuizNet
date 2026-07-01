using Microsoft.EntityFrameworkCore;
using QuizClient.Models.Entities.QuizRelevant;
using QuizClient.Models.QuizRelevant.Abstracts;
using QuizClient.Models.QuizRelevant.Entities.Questions;
using QuizClient.Models.UserRelevant;

namespace QuizClient.Models.DatabaseRelevant.Entities;

public class QuizDbContext : DbContext
{
    public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options) { }

    public DbSet<UserAccount> UserAccounts => Set<UserAccount>();

    public DbSet<Quiz> Quizzes => Set<Quiz>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<SingleChoiceQuestion> SingleChoiceQuestions => Set<SingleChoiceQuestion>();
    public DbSet<MultipleChoiceQuestion> MultipleChoiceQuestions => Set<MultipleChoiceQuestion>();
    public DbSet<QuestionOption> QuestionOptions => Set<QuestionOption>();
}