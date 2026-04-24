using Microsoft.EntityFrameworkCore;
using VisitTrack.Domain.Entities;

namespace VisitTrack.Infrastructure.Data;

public class VisitTrackDbContext : DbContext
{
    public VisitTrackDbContext(DbContextOptions<VisitTrackDbContext> options)
        : base(options) { }

    public DbSet<Visitante> Visitantes => Set<Visitante>();
    public DbSet<Area> Areas => Set<Area>();
    public DbSet<Empleado> Empleados => Set<Empleado>();
    public DbSet<Visita> Visitas => Set<Visita>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Visitante>(entity =>
        {
            entity.ToTable("Visitantes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Apellido).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DocumentoIdentidad).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.DocumentoIdentidad).IsUnique();
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Telefono).HasMaxLength(20);
            entity.Property(e => e.Empresa).HasMaxLength(150);
            entity.HasQueryFilter(e => e.IsActive);
        });

        modelBuilder.Entity<Area>(entity =>
        {
            entity.ToTable("Areas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Nombre).IsUnique();
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.HasQueryFilter(e => e.IsActive);
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.ToTable("Empleados");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Apellido).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Puesto).HasMaxLength(100);

            entity.HasOne(e => e.Area)
                .WithMany(a => a.Empleados)
                .HasForeignKey(e => e.AreaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(e => e.IsActive);
        });

        modelBuilder.Entity<Visita>(entity =>
        {
            entity.ToTable("Visitas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FechaEntrada).IsRequired();
            entity.Property(e => e.Motivo).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Estado).IsRequired().HasMaxLength(20).HasDefaultValue("Pendiente");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.Visitante)
                .WithMany(v => v.Visitas)
                .HasForeignKey(e => e.VisitanteId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.EmpleadoResponsable)
                .WithMany(emp => emp.Visitas)
                .HasForeignKey(e => e.EmpleadoResponsableId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Area)
                .WithMany(a => a.Visitas)
                .HasForeignKey(e => e.AreaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.User)
                .WithMany(u => u.Visitas)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.HasQueryFilter(e => e.IsActive);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Roles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Nombre).IsUnique();
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRoles");
            entity.HasKey(e => new { e.UserId, e.RoleId });

            entity.HasOne(e => e.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(e => e.UserId);

            entity.HasOne(e => e.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(e => e.RoleId);
        });
    }
}