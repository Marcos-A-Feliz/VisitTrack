// VisitTrack.Infrastructure/Repositories/VisitanteRepository.cs
using Microsoft.EntityFrameworkCore;
using VisitFlow.Domain.Entities;
using VisitFlow.Infrastructure.Contracts;
using VisitFlow.Infrastructure.Data;

namespace VisitFlow.Infrastructure.Repositories;

public class VisitanteRepository : GenericRepository<Visitante>, IVisitanteRepository
{
    public VisitanteRepository(VisitFlowDbContext context) : base(context) { }

    public async Task<Visitante?> GetByDocumentoAsync(string documentoIdentidad)
        => await _dbSet.FirstOrDefaultAsync(v => v.DocumentoIdentidad == documentoIdentidad);

    public async Task<IEnumerable<Visitante>> SearchAsync(string term)
        => await _dbSet
            .Where(v => v.Nombre.Contains(term) ||
                       v.Apellido.Contains(term) ||
                       v.DocumentoIdentidad.Contains(term))
            .ToListAsync();
}