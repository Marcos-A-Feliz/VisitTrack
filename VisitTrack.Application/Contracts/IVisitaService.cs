using VisitTrack.Application.DTOs;

namespace VisitTrack.Application.Contracts;

public interface IVisitaService
{
    Task<IEnumerable<VisitaDto>> GetAllAsync();
    Task<VisitaDto?> GetByIdAsync(int id);
    Task<IEnumerable<VisitaDto>> GetByVisitanteAsync(int visitanteId);
    Task<IEnumerable<VisitaDto>> GetByEmpleadoAsync(int empleadoId);
    Task<IEnumerable<VisitaDto>> GetByAreaAsync(int areaId);
    Task<IEnumerable<VisitaDto>> GetByFechaAsync(DateTime inicio, DateTime fin);
    Task<IEnumerable<VisitaDto>> GetActivasAsync();
    Task<VisitaDto> CreateAsync(CreateVisitaDto dto, int userId);
    Task<VisitaDto?> RegistrarSalidaAsync(int id);
    Task<bool> DeleteAsync(int id);
}