// VisitFlow.Infrastructure/Repositories/EmpleadoRepository.cs
using Microsoft.EntityFrameworkCore;
using VisitFlow.Domain.Entities;
using VisitFlow.Infrastructure.Contracts;
using VisitFlow.Infrastructure.Data;

namespace VisitFlow.Infrastructure.Repositories;

public class EmpleadoRepository : GenericRepository<Empleado>, IEmpleadoRepository
{
    public EmpleadoRepository(VisitFlowDbContext context) : base(context) { }

    public override async Task<IEnumerable<Empleado>> GetAllAsync()
        => await _dbSet.Include(e => e.Area).ToListAsync();

    public async Task<Empleado?> GetByEmailAsync(string email)
        => await _dbSet.Include(e => e.Area)
                       .FirstOrDefaultAsync(e => e.Email == email);

    public async Task<IEnumerable<Empleado>> GetByAreaAsync(int areaId)
        => await _dbSet.Include(e => e.Area)
                       .Where(e => e.AreaId == areaId)
                       .ToListAsync();
}