namespace VisitFlow.Domain.Entities;

public class Visita
{
    public int Id { get; set; }
    public DateTime FechaEntrada { get; set; } 
    public DateTime? FechaSalida { get; set; } 
    public required string Motivo { get; set; }
    public string Estado { get; set; } = "Pendiente";
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int VisitanteId { get; set; }
    public int EmpleadoResponsableId { get; set; }
    public int AreaId { get; set; }
    public int UserId { get; set; }

    public Visitante? Visitante { get; set; }
    public Empleado? EmpleadoResponsable { get; set; }
    public Area? Area { get; set; }
    public User? User { get; set; }
}