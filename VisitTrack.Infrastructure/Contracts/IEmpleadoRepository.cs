
using VisitTrack.Domain.Entities;

namespace VisitTrack.Infrastructure.Contracts;

public interface IEmpleadoRepository : IRepository<Empleado>
{
    Task<Empleado?> GetByEmailAsync(string email);
    Task<IEnumerable<Empleado>> GetByAreaAsync(int areaId);
}