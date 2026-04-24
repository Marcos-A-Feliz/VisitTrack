
using VisitTrack.Domain.Entities;

namespace VisitTrack.Infrastructure.Contracts;

public interface IVisitaRepository : IRepository<Visita>
{
    Task<IEnumerable<Visita>> GetByVisitanteAsync(int visitanteId);
    Task<IEnumerable<Visita>> GetByEmpleadoAsync(int empleadoId);
    Task<IEnumerable<Visita>> GetByAreaAsync(int areaId);
    Task<IEnumerable<Visita>> GetByEstadoAsync(string estado);
    Task<IEnumerable<Visita>> GetByFechaAsync(DateTime fechaInicio, DateTime fechaFin);
    Task<IEnumerable<Visita>> GetActivasAsync();
}
