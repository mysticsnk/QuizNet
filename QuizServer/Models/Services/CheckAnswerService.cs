using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizServer.Models.DatabaseRelevant.Interfaces;
using QuizServer.Models.AppRelevant;
using QuizServer.Models.DatabaseRelevant.Entities;
using QuizServer.Models.Entities.QuizRelevant;
using QuizServer.Models.Exceptions;
using QuizServer.Models.QuizRelevant.Abstracts;
using QuizServer.Models.QuizRelevant.Entities.Questions;
using QuizServer.Models.Services.Interfaces;
using QuizServer.Models.SessionRelevant.Answers;

namespace QuizServer.Models.Services;

public class CheckAnswerService : ICheckAnswerService
{
    public async Task<CheckAnswerResult> CheckAnswerAsync(Question question, Answer answer)
    {
        ServerState serverState = Program.AppHost.Services.GetRequiredService<ServerState>();
        IQuizRepository quizRepository = Program.AppHost.Services.GetRequiredService<IQuizRepository>();
        bool timeReducesPoints = serverState.CurrentSession.TimeReducesPoints;
        CheckAnswerResult result = new CheckAnswerResult();
        Console.WriteLine(question.Title);
        Console.WriteLine(answer.QuestionId);
        
        if (question is SingleChoiceQuestion singleChoiceQuestion &&
            answer is SingleChoiceAnswer singleChoiceAnswer)
        {
            QuestionOption? selectedOption = (await quizRepository.GetAllQuestionOptionsAsync())
                .FirstOrDefault(option => option.Id == singleChoiceAnswer.SelectedOptionId);

            if (selectedOption == null || !selectedOption.IsCorrect)
            {
                result.IsCorrect = false;
                result.PointsGained = 0;
                return result;
            }

            result.IsCorrect = true;
            if (timeReducesPoints)
            {
                result.PointsGained = CalculatePointsWithTime(question.PointsWeight,
                    question.TimeSpent.TotalSeconds, question.TimeLimit.TotalSeconds);
            }
            else
            {
                result.PointsGained = question.PointsWeight;
            }

            return result;
        }
        else if (question is MultiChoiceQuestion multiChoiceQuestion &&
                 answer is MultiChoiceAnswer multiChoiceAnswer)
        {
            HashSet<Guid> selectedIds = multiChoiceAnswer.SelectedOptionIds.ToHashSet();

            result.IsCorrect = true;

            int correctOptionsAmount = multiChoiceQuestion.Options.Count(o => o.IsCorrect);
            int pointsPerOption = question.PointsWeight / correctOptionsAmount;

            foreach (QuestionOption option in multiChoiceQuestion.Options)
            {
                bool selected = selectedIds.Contains(option.Id);

                if (option.IsCorrect)
                {
                    if (selected)
                    {
                        result.PointsGained += timeReducesPoints
                            ? CalculatePointsWithTime(pointsPerOption,
                                question.TimeSpent.TotalSeconds,
                                question.TimeLimit.TotalSeconds)
                            : pointsPerOption;
                    }
                    else
                    {
                        result.IsCorrect = false;
                    }
                }
                else
                {
                    if (selected)
                    {
                        result.PointsGained -= pointsPerOption;
                        result.IsCorrect = false;
                    }
                }
            }

            result.PointsGained = Math.Max(0, result.PointsGained);

            return result;
        }
        else if (question is ShortTextQuestion shortTextQuestion &&
                 answer is ShortTextAnswer shortTextAnswer)
        {
            if (shortTextQuestion.CaseSensitive ?? false)
            {
                if (shortTextAnswer.AnswerText != shortTextQuestion.CorrectText)
                {
                    result.IsCorrect = false;
                    result.PointsGained = 0;
                    return result;
                }
                
                result.IsCorrect = true;
        
                if (timeReducesPoints)
                {
                    result.PointsGained = CalculatePointsWithTime(question.PointsWeight,
                        question.TimeSpent.TotalSeconds, question.TimeLimit.TotalSeconds);  
                }
                else
                {
                    result.PointsGained = question.PointsWeight;
                }
            }
            else
            {
                if (shortTextAnswer.AnswerText.ToLower() != shortTextQuestion.CorrectText.ToLower())
                {
                    result.IsCorrect = false;
                    result.PointsGained = 0;
                    return result;
                }
                
                result.IsCorrect = true;
        
                if (timeReducesPoints)
                {
                    result.PointsGained = CalculatePointsWithTime(question.PointsWeight,
                        question.TimeSpent.TotalSeconds, question.TimeLimit.TotalSeconds);  
                }
                else
                {
                    result.PointsGained = question.PointsWeight;
                }
            }
        }
        else if (question is TrueFalseQuestion trueFalseQuestion &&
                 answer is TrueFalseAnswer trueFalseAnswer)
        {
            QuestionOption? selectedOption = (await quizRepository.GetAllQuestionOptionsAsync())
                .FirstOrDefault(option => option.Id == trueFalseAnswer.SelectedOptionId);

            if (selectedOption == null || !selectedOption.IsCorrect)
            {
                result.IsCorrect = false;
                result.PointsGained = 0;
            }

            result.IsCorrect = true;
            if (timeReducesPoints)
            {
                result.PointsGained = CalculatePointsWithTime(question.PointsWeight,
                    question.TimeSpent.TotalSeconds, question.TimeLimit.TotalSeconds);
            }
            else
            {
                result.PointsGained = question.PointsWeight;
            }
        }
        else
        {
            throw new QuestionAnswerMismatchException();
        }

        Console.WriteLine(result.IsCorrect);
        Console.WriteLine(result.PointsGained);
        return result;
    }

    private int CalculatePointsWithTime(int points, double timeSpent, double timeLimit)
    {
        double ratio = Math.Clamp(
            timeSpent / timeLimit,
            0d,
            1d
        );

        return (int)(points * (1 - ratio));
    }
}