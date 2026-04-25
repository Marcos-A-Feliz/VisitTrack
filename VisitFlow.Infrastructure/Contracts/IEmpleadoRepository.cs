
using VisitFlow.Domain.Entities;

namespace VisitFlow.Infrastructure.Contracts;

public interface IEmpleadoRepository : IRepository<Empleado>
{
    Task<Empleado?> GetByEmailAsync(string email);
    Task<IEnumerable<Empleado>> GetByAreaAsync(int areaId);
}