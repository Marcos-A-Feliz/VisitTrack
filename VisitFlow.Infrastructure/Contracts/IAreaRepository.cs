
using VisitFlow.Domain.Entities;

namespace VisitFlow.Infrastructure.Contracts;

public interface IAreaRepository : IRepository<Area>
{
    Task<Area?> GetByNombreAsync(string nombre);
}