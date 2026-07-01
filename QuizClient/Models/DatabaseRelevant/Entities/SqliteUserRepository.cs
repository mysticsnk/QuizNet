using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizClient.Models.DatabaseRelevant.Interfaces;
using QuizClient.Models.UserRelevant;

namespace QuizClient.Models.DatabaseRelevant.Entities;

public class SqliteUserRepository : IUserRepository
{
    private QuizDbContext _context { get; set; }

    public SqliteUserRepository(QuizDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<UserAccount>> GetAllUsersAsync()
    {
        return await _context.UserAccounts.ToListAsync();
    }

    public async Task<UserAccount?> GetUserByGuidAsync(Guid id)
    {
        return await _context.UserAccounts.FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task CreateAsync(UserAccount user)
    {
        await _context.UserAccounts.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(Guid guid)
    {
        UserAccount? user = await GetUserByGuidAsync(guid);
        if (user == null) return false;
        
        _context.UserAccounts.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}