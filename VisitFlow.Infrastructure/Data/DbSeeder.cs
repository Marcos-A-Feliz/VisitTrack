using Microsoft.EntityFrameworkCore;
using VisitFlow.Domain.Entities;

namespace VisitFlow.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task Initialize(VisitFlowDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (!await context.Roles.AnyAsync())
        {
            context.Roles.AddRange(
                new Role { Nombre = "Admin" },
                new Role { Nombre = "Guardia" },
                new Role { Nombre = "Empleado" }
            );
            await context.SaveChangesAsync();
        }

        if (!await context.Users.AnyAsync())
        {
            var adminRole = await context.Roles.FirstAsync(r => r.Nombre == "Admin");
            var guardiaRole = await context.Roles.FirstAsync(r => r.Nombre == "Guardia");

            var adminHash = BCrypt.Net.BCrypt.HashPassword("Admin123!");
            var guardiaHash = BCrypt.Net.BCrypt.HashPassword("Guardia123!");

            context.Users.AddRange(
                new User
                {
                    FirstName = "Admin",
                    LastName = "Sistema",
                    Email = "admin@visittrack.com",
                    PasswordHash = adminHash,
                    UserRoles = new List<UserRole> { new() { Role = adminRole } }
                },
                new User
                {
                    FirstName = "Pedro",
                    LastName = "Seguridad",
                    Email = "guardia@visittrack.com",
                    PasswordHash = guardiaHash,
                    UserRoles = new List<UserRole> { new() { Role = guardiaRole } }
                }
            );
            await context.SaveChangesAsync();
        }

        if (!await context.Areas.AnyAsync())
        {
            context.Areas.AddRange(
                new Area { Nombre = "Recursos Humanos", Descripcion = "Gestión de personal" },
                new Area { Nombre = "Tecnología", Descripcion = "Sistemas y soporte" },
                new Area { Nombre = "Contabilidad", Descripcion = "Finanzas y pagos" },
                new Area { Nombre = "Gerencia General", Descripcion = "Dirección ejecutiva" }
            );
            await context.SaveChangesAsync();
        }

        if (!await context.Empleados.AnyAsync())
        {
            var areas = await context.Areas.ToListAsync();
            context.Empleados.AddRange(
                new Empleado { Nombre = "Carlos", Apellido = "González", Email = "carlos@visittrack.com", Puesto = "Analista", AreaId = areas[1].Id },
                new Empleado { Nombre = "María", Apellido = "López", Email = "maria@visittrack.com", Puesto = "Directora RRHH", AreaId = areas[0].Id },
                new Empleado { Nombre = "José", Apellido = "Martínez", Email = "jose@visittrack.com", Puesto = "Contador", AreaId = areas[2].Id },
                new Empleado { Nombre = "Laura", Apellido = "Fernández", Email = "laura@visittrack.com", Puesto = "Gerente", AreaId = areas[3].Id }
            );
            await context.SaveChangesAsync();
        }

        if (!await context.Visitantes.AnyAsync())
        {
            context.Visitantes.AddRange(
                new Visitante { Nombre = "Juan", Apellido = "Pérez", DocumentoIdentidad = "001-1234567-8", Email = "juan@gmail.com", Telefono = "809-555-0101", Empresa = "Tech Solutions" },
                new Visitante { Nombre = "Ana", Apellido = "Rodríguez", DocumentoIdentidad = "002-2345678-9", Email = "ana@hotmail.com", Telefono = "809-555-0202" }
            );
            await context.SaveChangesAsync();
        }

        if (!await context.Visitas.AnyAsync())
        {
            var visitante = await context.Visitantes.FirstAsync();
            var empleado = await context.Empleados.FirstAsync(e => e.Email == "carlos@visittrack.com");
            var area = await context.Areas.FirstAsync(a => a.Nombre == "Tecnología");
            var guardia = await context.Users.FirstAsync(u => u.Email == "guardia@visittrack.com");

            context.Visitas.Add(new Visita
            {
                FechaEntrada = DateTime.Now.AddHours(-1),
                Motivo = "Reunión de proyecto",
                Estado = "EnCurso",
                VisitanteId = visitante.Id,
                EmpleadoResponsableId = empleado.Id,
                AreaId = area.Id,
                UserId = guardia.Id
            });
            await context.SaveChangesAsync();
        }
    }
}