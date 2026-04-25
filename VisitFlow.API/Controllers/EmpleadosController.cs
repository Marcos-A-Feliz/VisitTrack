using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisitFlow.Application.Contracts;
using VisitFlow.Application.DTOs;

namespace VisitFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmpleadosController : ControllerBase
{
    private readonly IEmpleadoService _svc;
    public EmpleadosController(IEmpleadoService svc) => _svc = svc;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _svc.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var r = await _svc.GetByIdAsync(id);
        return r == null ? NotFound() : Ok(r);
    }

    [HttpGet("area/{areaId}")]
    public async Task<IActionResult> GetByArea(int areaId)
        => Ok(await _svc.GetByAreaAsync(areaId));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmpleadoDto dto)
    {
        var r = await _svc.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = r.Id }, r);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmpleadoDto dto)
    {
        var r = await _svc.UpdateAsync(id, dto);
        return r == null ? NotFound() : Ok(r);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _svc.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }
}
