namespace VisitTrack.Domain.Entities;

public class Area
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public string? Descripcion { get; set; }
    public bool IsActive { get; set; } = true;
    public List<Empleado> Empleados { get; set; } = [];
    public List<Visita> Visitas { get; set; } = [];
}