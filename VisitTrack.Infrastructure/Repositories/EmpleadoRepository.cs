using Microsoft.EntityFrameworkCore;
using VisitTrack.Domain.Entities;
using VisitTrack.Infrastructure.Contracts;
using VisitTrack.Infrastructure.Data;

namespace VisitTrack.Infrastructure.Repositories;

public class EmpleadoRepository : GenericRepository<Empleado>, IEmpleadoRepository
{
    public EmpleadoRepository(VisitTrackDbContext context) : base(context) { }

    public async Task<Empleado?> GetByEmailAsync(string email)
        => await _dbSet.Include(e => e.Area)
                       .FirstOrDefaultAsync(e => e.Email == email);

    public async Task<IEnumerable<Empleado>> GetByAreaAsync(int areaId)
        => await _dbSet.Include(e => e.Area)
                       .Where(e => e.AreaId == areaId)
                       .ToListAsync();
}