using Microsoft.EntityFrameworkCore;
using VisitFlow.Domain.Entities;
using VisitFlow.Infrastructure.Contracts;
using VisitFlow.Infrastructure.Data;

namespace VisitFlow.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(VisitFlowDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email)
        => await _dbSet.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User?> GetByEmailWithRolesAsync(string email)
        => await _dbSet.Include(u => u.UserRoles)
                       .ThenInclude(ur => ur.Role)
                       .FirstOrDefaultAsync(u => u.Email == email);

    public async Task<bool> EmailExistsAsync(string email)
        => await _dbSet.AnyAsync(u => u.Email == email);
}