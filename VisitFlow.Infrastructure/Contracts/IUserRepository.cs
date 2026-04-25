
using VisitFlow.Domain.Entities;

namespace VisitFlow.Infrastructure.Contracts;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByEmailWithRolesAsync(string email);
    Task<bool> EmailExistsAsync(string email);
}