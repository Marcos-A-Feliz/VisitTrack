
using VisitTrack.Domain.Entities;

namespace VisitTrack.Infrastructure.Contracts;

public interface IVisitanteRepository : IRepository<Visitante>
{
    Task<Visitante?> GetByDocumentoAsync(string documentoIdentidad);
    Task<IEnumerable<Visitante>> SearchAsync(string term);
}