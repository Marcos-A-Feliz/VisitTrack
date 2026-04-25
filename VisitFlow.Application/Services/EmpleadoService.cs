using AutoMapper;
using VisitFlow.Application.Contracts;
using VisitFlow.Application.DTOs;
using VisitFlow.Domain.Entities;
using VisitFlow.Infrastructure.Contracts;

namespace VisitFlow.Application.Services;

public class EmpleadoService : IEmpleadoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EmpleadoService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EmpleadoDto>> GetAllAsync()
    {
        var empleados = await _unitOfWork.Empleados.GetAllAsync();
        return _mapper.Map<IEnumerable<EmpleadoDto>>(empleados);
    }

    public async Task<EmpleadoDto?> GetByIdAsync(int id)
    {
        var empleado = await _unitOfWork.Empleados.GetByIdAsync(id);
        return empleado == null ? null : _mapper.Map<EmpleadoDto>(empleado);
    }

    public async Task<IEnumerable<EmpleadoDto>> GetByAreaAsync(int areaId)
    {
        var empleados = await _unitOfWork.Empleados.GetByAreaAsync(areaId);
        return _mapper.Map<IEnumerable<EmpleadoDto>>(empleados);
    }

    public async Task<EmpleadoDto> CreateAsync(CreateEmpleadoDto dto)
    {
        var empleado = _mapper.Map<Empleado>(dto);
        await _unitOfWork.Empleados.AddAsync(empleado);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<EmpleadoDto>(empleado);
    }

    public async Task<EmpleadoDto?> UpdateAsync(int id, UpdateEmpleadoDto dto)
    {
        var empleado = await _unitOfWork.Empleados.GetByIdAsync(id);
        if (empleado == null) return null;

        _mapper.Map(dto, empleado);
        await _unitOfWork.Empleados.UpdateAsync(empleado);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<EmpleadoDto>(empleado);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var empleado = await _unitOfWork.Empleados.GetByIdAsync(id);
        if (empleado == null) return false;

        empleado.IsActive = false;
        await _unitOfWork.Empleados.UpdateAsync(empleado);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}