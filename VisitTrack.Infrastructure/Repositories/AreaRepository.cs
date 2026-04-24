using Microsoft.EntityFrameworkCore;
using VisitTrack.Domain.Entities;
using VisitTrack.Infrastructure.Contracts;
using VisitTrack.Infrastructure.Data;

namespace VisitTrack.Infrastructure.Repositories;

public class AreaRepository : GenericRepository<Area>, IAreaRepository
{
    public AreaRepository(VisitTrackDbContext context) : base(context) { }

    public async Task<Area?> GetByNombreAsync(string nombre)
        => await _dbSet.FirstOrDefaultAsync(a => a.Nombre == nombre);
}