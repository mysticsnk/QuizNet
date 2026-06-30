using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuizServer.Models.Entities.QuizRelevant;

namespace QuizServer.Models.DatabaseRelevant.Interfaces;

public interface IQuizRepository
{
    public Task<List<Quiz>> GetAllQuizzesAsync();
    public Task<Quiz?> GetQuizByGuidAsync(Guid id);
    public Task CreateAsync(Quiz quiz);
    public Task<bool> DeleteAsync(Guid id);
}