// VisitTrack.Application/Mappings/MappingProfile.cs
using AutoMapper;
using VisitTrack.Application.DTOs;
using VisitTrack.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VisitTrack.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ========== AREA ==========
        CreateMap<Area, AreaDto>();
        CreateMap<CreateAreaDto, Area>();
        CreateMap<UpdateAreaDto, Area>();

        // ========== EMPLEADO ==========
        CreateMap<Empleado, EmpleadoDto>()
            .ForMember(dest => dest.AreaNombre,
                       opt => opt.MapFrom(src => src.Area != null ? src.Area.Nombre : string.Empty));
        CreateMap<CreateEmpleadoDto, Empleado>();
        CreateMap<UpdateEmpleadoDto, Empleado>();

        // ========== VISITANTE ==========
        CreateMap<Visitante, VisitanteDto>();
        CreateMap<CreateVisitanteDto, Visitante>();
        CreateMap<UpdateVisitanteDto, Visitante>();

        // ========== VISITA ==========
        CreateMap<Visita, VisitaDto>()
            .ForMember(dest => dest.VisitanteNombre,
                       opt => opt.MapFrom(src => $"{src.Visitante!.Nombre} {src.Visitante.Apellido}"))
            .ForMember(dest => dest.VisitanteDocumento,
                       opt => opt.MapFrom(src => src.Visitante!.DocumentoIdentidad))
            .ForMember(dest => dest.EmpleadoNombre,
                       opt => opt.MapFrom(src => $"{src.EmpleadoResponsable!.Nombre} {src.EmpleadoResponsable.Apellido}"))
            .ForMember(dest => dest.AreaNombre,
                       opt => opt.MapFrom(src => src.Area != null ? src.Area.Nombre : string.Empty))
            .ForMember(dest => dest.UserNombre,
                       opt => opt.MapFrom(src => $"{src.User!.FirstName} {src.User.LastName}"));

        CreateMap<CreateVisitaDto, Visita>();
        CreateMap<UpdateVisitaDto, Visita>();
    }
}