using Microsoft.EntityFrameworkCore;
using VisitTrack.Domain.Entities;
using VisitTrack.Infrastructure.Contracts;
using VisitTrack.Infrastructure.Data;

namespace VisitTrack.Infrastructure.Repositories;

public class VisitaRepository : GenericRepository<Visita>, IVisitaRepository
{
    public VisitaRepository(VisitTrackDbContext context) : base(context) { }

    private IQueryable<Visita> BaseQuery()
        => _dbSet
            .Include(v => v.Visitante)
            .Include(v => v.EmpleadoResponsable)
            .Include(v => v.Area)
            .Include(v => v.User);

    public override async Task<IEnumerable<Visita>> GetAllAsync()
        => await BaseQuery().ToListAsync();

    public override async Task<Visita?> GetByIdAsync(int id)
        => await BaseQuery().FirstOrDefaultAsync(v => v.Id == id);

    public async Task<IEnumerable<Visita>> GetByVisitanteAsync(int visitanteId)
        => await BaseQuery().Where(v => v.VisitanteId == visitanteId).ToListAsync();

    public async Task<IEnumerable<Visita>> GetByEmpleadoAsync(int empleadoId)
        => await BaseQuery().Where(v => v.EmpleadoResponsableId == empleadoId).ToListAsync();

    public async Task<IEnumerable<Visita>> GetByAreaAsync(int areaId)
        => await BaseQuery().Where(v => v.AreaId == areaId).ToListAsync();

    public async Task<IEnumerable<Visita>> GetByEstadoAsync(string estado)
        => await BaseQuery().Where(v => v.Estado == estado).ToListAsync();

    public async Task<IEnumerable<Visita>> GetByFechaAsync(DateTime fechaInicio, DateTime fechaFin)
        => await BaseQuery()
            .Where(v => v.FechaEntrada >= fechaInicio && v.FechaEntrada <= fechaFin)
            .ToListAsync();

    public async Task<IEnumerable<Visita>> GetActivasAsync()
        => await BaseQuery()
            .Where(v => v.Estado == "EnCurso" || v.Estado == "Pendiente")
            .ToListAsync();
}
