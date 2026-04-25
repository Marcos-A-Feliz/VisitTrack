
using VisitFlow.Domain.Entities;

namespace VisitFlow.Infrastructure.Contracts;

public interface IVisitanteRepository : IRepository<Visitante>
{
    Task<Visitante?> GetByDocumentoAsync(string documentoIdentidad);
    Task<IEnumerable<Visitante>> SearchAsync(string term);
}