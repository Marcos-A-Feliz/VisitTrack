using AutoMapper;
using VisitTrack.Application.Contracts;
using VisitTrack.Application.DTOs;
using VisitTrack.Domain.Entities;
using VisitTrack.Infrastructure.Contracts;

namespace VisitTrack.Application.Services;

public class VisitanteService : IVisitanteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VisitanteService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VisitanteDto>> GetAllAsync()
    {
        var visitantes = await _unitOfWork.Visitantes.GetAllAsync();
        return _mapper.Map<IEnumerable<VisitanteDto>>(visitantes);
    }

    public async Task<VisitanteDto?> GetByIdAsync(int id)
    {
        var visitante = await _unitOfWork.Visitantes.GetByIdAsync(id);
        return visitante == null ? null : _mapper.Map<VisitanteDto>(visitante);
    }

    public async Task<IEnumerable<VisitanteDto>> SearchAsync(string term)
    {
        var visitantes = await _unitOfWork.Visitantes.SearchAsync(term);
        return _mapper.Map<IEnumerable<VisitanteDto>>(visitantes);
    }

    public async Task<VisitanteDto> CreateAsync(CreateVisitanteDto dto)
    {
        var visitante = _mapper.Map<Visitante>(dto);
        await _unitOfWork.Visitantes.AddAsync(visitante);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<VisitanteDto>(visitante);
    }

    public async Task<VisitanteDto?> UpdateAsync(int id, UpdateVisitanteDto dto)
    {
        var visitante = await _unitOfWork.Visitantes.GetByIdAsync(id);
        if (visitante == null) return null;

        _mapper.Map(dto, visitante);
        await _unitOfWork.Visitantes.UpdateAsync(visitante);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<VisitanteDto>(visitante);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var visitante = await _unitOfWork.Visitantes.GetByIdAsync(id);
        if (visitante == null) return false;

        visitante.IsActive = false;
        await _unitOfWork.Visitantes.UpdateAsync(visitante);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}