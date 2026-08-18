using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuizServer.Models.Entities.QuizRelevant;
using QuizServer.Models.QuizRelevant.Abstracts;

namespace QuizServer.Models.DatabaseRelevant.Interfaces;

public interface IQuizRepository
{
    public Task<List<Quiz>> GetAllQuizzesAsync();
    public Task<Quiz?> GetQuizByGuidAsync(Guid id);
    public Task CreateAsync(Quiz quiz);
    public Task<bool> DeleteAsync(Guid id);
    public Task<List<Question>> GetAllQuestionsAsync();
    public Task<List<QuestionOption>> GetAllQuestionOptionsAsync();
}