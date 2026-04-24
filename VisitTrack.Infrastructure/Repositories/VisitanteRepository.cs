// VisitTrack.Infrastructure/Repositories/VisitanteRepository.cs
using Microsoft.EntityFrameworkCore;
using VisitTrack.Domain.Entities;
using VisitTrack.Infrastructure.Contracts;
using VisitTrack.Infrastructure.Data;

namespace VisitTrack.Infrastructure.Repositories;

public class VisitanteRepository : GenericRepository<Visitante>, IVisitanteRepository
{
    public VisitanteRepository(VisitTrackDbContext context) : base(context) { }

    public async Task<Visitante?> GetByDocumentoAsync(string documentoIdentidad)
        => await _dbSet.FirstOrDefaultAsync(v => v.DocumentoIdentidad == documentoIdentidad);

    public async Task<IEnumerable<Visitante>> SearchAsync(string term)
        => await _dbSet
            .Where(v => v.Nombre.Contains(term) ||
                       v.Apellido.Contains(term) ||
                       v.DocumentoIdentidad.Contains(term))
            .ToListAsync();
}