using AutoMapper;
using VisitFlow.Application.Contracts;
using VisitFlow.Application.DTOs;
using VisitFlow.Domain.Entities;
using VisitFlow.Infrastructure.Contracts;

namespace VisitFlow.Application.Services;

public class AreaService : IAreaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AreaService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AreaDto>> GetAllAsync()
    {
        var areas = await _unitOfWork.Areas.GetAllAsync();
        return _mapper.Map<IEnumerable<AreaDto>>(areas);
    }

    public async Task<AreaDto?> GetByIdAsync(int id)
    {
        var area = await _unitOfWork.Areas.GetByIdAsync(id);
        return area == null ? null : _mapper.Map<AreaDto>(area);
    }

    public async Task<AreaDto> CreateAsync(CreateAreaDto dto)
    {
        var area = _mapper.Map<Area>(dto);
        await _unitOfWork.Areas.AddAsync(area);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<AreaDto>(area);
    }

    public async Task<AreaDto?> UpdateAsync(int id, UpdateAreaDto dto)
    {
        var area = await _unitOfWork.Areas.GetByIdAsync(id);
        if (area == null) return null;

        _mapper.Map(dto, area);
        await _unitOfWork.Areas.UpdateAsync(area);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<AreaDto>(area);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var area = await _unitOfWork.Areas.GetByIdAsync(id);
        if (area == null) return false;

        area.IsActive = false;
        await _unitOfWork.Areas.UpdateAsync(area);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}