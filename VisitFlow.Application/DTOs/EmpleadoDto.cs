namespace VisitFlow.Application.DTOs;

public class EmpleadoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Puesto { get; set; }
    public int AreaId { get; set; }
    public string AreaNombre { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class CreateEmpleadoDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Puesto { get; set; }
    public int AreaId { get; set; }
}

public class UpdateEmpleadoDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Puesto { get; set; }
    public int AreaId { get; set; }
}