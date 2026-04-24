namespace VisitTrack.Application.DTOs;

public class VisitaDto
{
    public int Id { get; set; }
    public DateTime FechaEntrada { get; set; }
    public DateTime? FechaSalida { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public int VisitanteId { get; set; }
    public string VisitanteNombre { get; set; } = string.Empty;
    public string VisitanteDocumento { get; set; } = string.Empty;

    public int EmpleadoResponsableId { get; set; }
    public string EmpleadoNombre { get; set; } = string.Empty;

    public int AreaId { get; set; }
    public string AreaNombre { get; set; } = string.Empty;

    public int UserId { get; set; }
    public string UserNombre { get; set; } = string.Empty;
}

public class CreateVisitaDto
{
    public string Motivo { get; set; } = string.Empty;
    public int VisitanteId { get; set; }
    public int EmpleadoResponsableId { get; set; }
    public int AreaId { get; set; }
}

public class UpdateVisitaDto
{
    public string? Motivo { get; set; }
    public int? EmpleadoResponsableId { get; set; }
    public int? AreaId { get; set; }
}