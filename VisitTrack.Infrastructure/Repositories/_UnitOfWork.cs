// VisitTrack.Infrastructure/Repositories/UnitOfWork.cs
using VisitTrack.Infrastructure.Contracts;
using VisitTrack.Infrastructure.Data;

namespace VisitTrack.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly VisitTrackDbContext _context;

    private IVisitanteRepository? _visitantes;
    private IAreaRepository? _areas;
    private IEmpleadoRepository? _empleados;
    private IVisitaRepository? _visitas;
    private IUserRepository? _users;

    public UnitOfWork(VisitTrackDbContext context)
    {
        _context = context;
    }

    public IVisitanteRepository Visitantes =>
        _visitantes ??= new VisitanteRepository(_context);

    public IAreaRepository Areas =>
        _areas ??= new AreaRepository(_context);

    public IEmpleadoRepository Empleados =>
        _empleados ??= new EmpleadoRepository(_context);

    public IVisitaRepository Visitas =>
        _visitas ??= new VisitaRepository(_context);

    public IUserRepository Users =>
        _users ??= new UserRepository(_context);

    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public void Dispose()
        => _context.Dispose();
}
