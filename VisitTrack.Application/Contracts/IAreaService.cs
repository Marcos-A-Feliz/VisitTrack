using VisitTrack.Application.DTOs;

namespace VisitTrack.Application.Contracts;

public interface IAreaService
{
    Task<IEnumerable<AreaDto>> GetAllAsync();
    Task<AreaDto?> GetByIdAsync(int id);
    Task<AreaDto> CreateAsync(CreateAreaDto dto);
    Task<AreaDto?> UpdateAsync(int id, UpdateAreaDto dto);
    Task<bool> DeleteAsync(int id);
}