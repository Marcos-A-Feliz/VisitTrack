using VisitTrack.Application.DTOs;

namespace VisitTrack.Application.Contracts;

public interface IVisitanteService
{
    Task<IEnumerable<VisitanteDto>> GetAllAsync();
    Task<VisitanteDto?> GetByIdAsync(int id);
    Task<IEnumerable<VisitanteDto>> SearchAsync(string term);
    Task<VisitanteDto> CreateAsync(CreateVisitanteDto dto);
    Task<VisitanteDto?> UpdateAsync(int id, UpdateVisitanteDto dto);
    Task<bool> DeleteAsync(int id);
}
