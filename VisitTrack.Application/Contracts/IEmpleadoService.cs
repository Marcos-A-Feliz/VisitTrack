using VisitTrack.Application.DTOs;

namespace VisitTrack.Application.Contracts;

public interface IEmpleadoService
{
    Task<IEnumerable<EmpleadoDto>> GetAllAsync();
    Task<EmpleadoDto?> GetByIdAsync(int id);
    Task<IEnumerable<EmpleadoDto>> GetByAreaAsync(int areaId);
    Task<EmpleadoDto> CreateAsync(CreateEmpleadoDto dto);
    Task<EmpleadoDto?> UpdateAsync(int id, UpdateEmpleadoDto dto);
    Task<bool> DeleteAsync(int id);
}