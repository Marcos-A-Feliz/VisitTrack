namespace VisitFlow.Domain.Entities;

public class Empleado 
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public required string Apellido { get; set; }
    public required string Email { get; set; }
    public string? Puesto { get; set; }
    public int AreaId { get; set; }
    public bool IsActive { get; set; } = true;
    public Area? Area { get; set; }
    public List<Visita> Visitas { get; set; } = [];
}