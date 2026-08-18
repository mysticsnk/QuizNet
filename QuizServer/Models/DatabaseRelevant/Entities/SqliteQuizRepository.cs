using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizServer.Models.DatabaseRelevant.Interfaces;
using QuizServer.Models.Entities.QuizRelevant;
using QuizServer.Models.QuizRelevant.Abstracts;

namespace QuizServer.Models.DatabaseRelevant.Entities;

public class SqliteQuizRepository : IQuizRepository
{
    private QuizDbContext _context { get; set; }

    public SqliteQuizRepository(QuizDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Quiz>> GetAllQuizzesAsync()
    {
        return await _context.Quizzes
            .Include(Quiz => Quiz.Questions)
            .ThenInclude(question => question.Options)
            .ToListAsync();
    }

    public async Task UpdateAsync(Quiz quiz)
    {
        var existingQuiz = await _context.Quizzes
            .Include(q => q.Questions)
            .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == quiz.Id);

        if (existingQuiz == null)
            throw new Exception("Quiz not found");

        // update basic fields
        existingQuiz.Title = quiz.Title;

        // remove old questions
        _context.Questions.RemoveRange(existingQuiz.Questions);

        // attach new questions
        existingQuiz.Questions = quiz.Questions;

        await _context.SaveChangesAsync();
    }
    public async Task<List<Question>> GetAllQuestionsAsync()
    {
        return await _context.Questions.Include(question => question.Options).ToListAsync();
    }

    public async Task<List<QuestionOption>> GetAllQuestionOptionsAsync()
    {
        return await _context.QuestionOptions.ToListAsync();
    }

    public async Task<Quiz?> GetQuizByGuidAsync(Guid id)
    {
        return await _context.Quizzes.Include(q => q.Questions).ThenInclude(question => question.Options).FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task CreateAsync(Quiz quiz)
    {
        Console.WriteLine($"Saving quiz {quiz.Title} with ID {quiz.Id}");
        _context.Quizzes.Add(quiz);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        Quiz? quiz = await GetQuizByGuidAsync(id);

        if (quiz == null) return false;

        _context.Quizzes.Remove(quiz);
        await _context.SaveChangesAsync();
        return true;
    }
}