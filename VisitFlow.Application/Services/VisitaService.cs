using AutoMapper;
using VisitFlow.Application.Contracts;
using VisitFlow.Application.DTOs;
using VisitFlow.Domain.Entities;
using VisitFlow.Infrastructure.Contracts;

namespace VisitFlow.Application.Services;

public class VisitaService : IVisitaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VisitaService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VisitaDto>> GetAllAsync()
    {
        var visitas = await _unitOfWork.Visitas.GetAllAsync();
        return _mapper.Map<IEnumerable<VisitaDto>>(visitas);
    }

    public async Task<VisitaDto?> GetByIdAsync(int id)
    {
        var visita = await _unitOfWork.Visitas.GetByIdAsync(id);
        return visita == null ? null : _mapper.Map<VisitaDto>(visita);
    }

    public async Task<IEnumerable<VisitaDto>> GetByVisitanteAsync(int visitanteId)
    {
        var visitas = await _unitOfWork.Visitas.GetByVisitanteAsync(visitanteId);
        return _mapper.Map<IEnumerable<VisitaDto>>(visitas);
    }

    public async Task<IEnumerable<VisitaDto>> GetByEmpleadoAsync(int empleadoId)
    {
        var visitas = await _unitOfWork.Visitas.GetByEmpleadoAsync(empleadoId);
        return _mapper.Map<IEnumerable<VisitaDto>>(visitas);
    }

    public async Task<IEnumerable<VisitaDto>> GetByAreaAsync(int areaId)
    {
        var visitas = await _unitOfWork.Visitas.GetByAreaAsync(areaId);
        return _mapper.Map<IEnumerable<VisitaDto>>(visitas);
    }

    public async Task<IEnumerable<VisitaDto>> GetByFechaAsync(DateTime inicio, DateTime fin)
    {
        var visitas = await _unitOfWork.Visitas.GetByFechaAsync(inicio, fin);
        return _mapper.Map<IEnumerable<VisitaDto>>(visitas);
    }

    public async Task<IEnumerable<VisitaDto>> GetActivasAsync()
    {
        var visitas = await _unitOfWork.Visitas.GetActivasAsync();
        return _mapper.Map<IEnumerable<VisitaDto>>(visitas);
    }

    public async Task<VisitaDto> CreateAsync(CreateVisitaDto dto, int userId)
    {
        var visita = _mapper.Map<Visita>(dto);
        visita.FechaEntrada = DateTime.Now;
        visita.Estado = "EnCurso";
        visita.UserId = userId;

        await _unitOfWork.Visitas.AddAsync(visita);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<VisitaDto>(visita);
    }

    public async Task<VisitaDto?> RegistrarSalidaAsync(int id)
    {
        var visita = await _unitOfWork.Visitas.GetByIdAsync(id);
        if (visita == null) return null;

        visita.FechaSalida = DateTime.UtcNow;
        visita.Estado = "Finalizada";

        await _unitOfWork.Visitas.UpdateAsync(visita);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<VisitaDto>(visita);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var visita = await _unitOfWork.Visitas.GetByIdAsync(id);
        if (visita == null) return false;

        visita.Estado = "Cancelada";
        await _unitOfWork.Visitas.UpdateAsync(visita);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}