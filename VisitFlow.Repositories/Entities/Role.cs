namespace VisitTrack.Domain.Entities;

public class Role
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public List<UserRole> UserRoles { get; set; } = [];
}
