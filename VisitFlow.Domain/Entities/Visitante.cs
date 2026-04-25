

namespace VisitFlow.Domain.Entities
{
    public class Visitante
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string DocumentoIdentidad { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Empresa { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<Visita> Visitas { get; set; } = new List<Visita>();
    }
}
