
using VisitTrack.Domain.Entities;

namespace VisitTrack.Infrastructure.Contracts;

public interface IAreaRepository : IRepository<Area>
{
    Task<Area?> GetByNombreAsync(string nombre);
}