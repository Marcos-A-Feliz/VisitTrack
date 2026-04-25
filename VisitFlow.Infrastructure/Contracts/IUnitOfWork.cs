
namespace VisitFlow.Infrastructure.Contracts;

public interface IUnitOfWork : IDisposable
{
    IVisitanteRepository Visitantes { get; }
    IAreaRepository Areas { get; }
    IEmpleadoRepository Empleados { get; }
    IVisitaRepository Visitas { get; }
    IUserRepository Users { get; }

    Task<int> SaveChangesAsync();
}