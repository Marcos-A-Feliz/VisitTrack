using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VisitFlow.Application.Contracts;
using VisitFlow.Application.DTOs;
using VisitFlow.Application.Services;

namespace VisitFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VisitasController : ControllerBase
{
    private readonly IVisitaService _svc;
    public VisitasController(IVisitaService svc) => _svc = svc;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _svc.GetAllAsync());

    [HttpGet("activas")]
    public async Task<IActionResult> GetActivas()
        => Ok(await _svc.GetActivasAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var r = await _svc.GetByIdAsync(id);
        return r == null ? NotFound() : Ok(r);
    }

    [HttpGet("visitante/{visitanteId}")]
    public async Task<IActionResult> GetByVisitante(int visitanteId)
        => Ok(await _svc.GetByVisitanteAsync(visitanteId));

    [HttpGet("empleado/{empleadoId}")]
    public async Task<IActionResult> GetByEmpleado(int empleadoId)
        => Ok(await _svc.GetByEmpleadoAsync(empleadoId));

    [HttpGet("area/{areaId}")]
    public async Task<IActionResult> GetByArea(int areaId)
        => Ok(await _svc.GetByAreaAsync(areaId));

    [HttpGet("reporte")]
    public async Task<IActionResult> GetReporte([FromQuery] DateTime inicio, [FromQuery] DateTime fin)
        => Ok(await _svc.GetByFechaAsync(inicio, fin));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVisitaDto dto)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

        var r = await _svc.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = r.Id }, r);
    }

    [HttpPost("{id}/salida")]
    public async Task<IActionResult> Salida(int id)
    {
        var r = await _svc.RegistrarSalidaAsync(id);
        return r == null ? NotFound() : Ok(r);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _svc.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }

    [HttpPut("{id}/cancelar")]
    [Authorize(Roles = "Admin,Guardia")]
    public async Task<IActionResult> Cancelar(int id)
    {
        var visita = await _svc.CancelarAsync(id);
        if (visita == null)
            return NotFound(new { message = "Visita no encontrada" });
        return Ok(new { message = "Visita cancelada correctamente", visita });
    }
}
