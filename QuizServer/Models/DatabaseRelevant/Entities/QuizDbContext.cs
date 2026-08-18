using Microsoft.EntityFrameworkCore;
using QuizServer.Models.Entities.QuizRelevant;
using QuizServer.Models.QuizRelevant.Abstracts;
using QuizServer.Models.QuizRelevant.Entities.Questions;
using QuizServer.Models.SessionRelevant.Answers;
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
    public DbSet<ShortTextQuestion> ShortTextQuestions => Set<ShortTextQuestion>();
    public DbSet<TrueFalseQuestion> TrueFalseQuestions => Set<TrueFalseQuestion>();
    
    public DbSet<SingleChoiceAnswer> SingleChoiceAnswers => Set<SingleChoiceAnswer>();
    public DbSet<MultiChoiceAnswer> MultiChoiceAnswers => Set<MultiChoiceAnswer>();
    public DbSet<ShortTextAnswer> ShortTextAnswers => Set<ShortTextAnswer>();
    public DbSet<TrueFalseAnswer> TrueFalseAnswers => Set<TrueFalseAnswer>();
    
    public DbSet<QuestionOption> QuestionOptions => Set<QuestionOption>();
}