using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuizClient.Models.UserRelevant;

namespace QuizClient.Models.DatabaseRelevant.Interfaces;

public interface IUserRepository
{
    public Task<List<UserAccount>> GetAllUsersAsync();
    public Task<UserAccount?> GetUserByGuidAsync(Guid id);
    public Task CreateAsync(UserAccount user);
    public Task<bool> DeleteAsync(Guid guid);
}