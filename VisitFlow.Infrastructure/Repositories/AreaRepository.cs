using Microsoft.EntityFrameworkCore;
using VisitFlow.Domain.Entities;
using VisitFlow.Infrastructure.Contracts;
using VisitFlow.Infrastructure.Data;

namespace VisitFlow.Infrastructure.Repositories;

public class AreaRepository : GenericRepository<Area>, IAreaRepository
{
    public AreaRepository(VisitFlowDbContext context) : base(context) { }

    public async Task<Area?> GetByNombreAsync(string nombre)
        => await _dbSet.FirstOrDefaultAsync(a => a.Nombre == nombre);
}