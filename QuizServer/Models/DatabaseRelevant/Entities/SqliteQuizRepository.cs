using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizServer.Models.DatabaseRelevant.Interfaces;
using QuizServer.Models.Entities.QuizRelevant;

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
        return await _context.Quizzes.ToListAsync();
    }

    public async Task<Quiz?> GetQuizByGuidAsync(Guid id)
    {
        return await _context.Quizzes.Include(q => q.Questions).FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task CreateAsync(Quiz quiz)
    {
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