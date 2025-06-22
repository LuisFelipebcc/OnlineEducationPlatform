using AutoMapper;
using ContentManagement.Application.DTOs;
using ContentManagement.Domain.Entities;
using ContentManagement.Domain.ValueObjects; // Namespace da sua camada de domínio deve ser referenciado

namespace ContentManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeamento de DTO de criação para entidade de domínio
            CreateMap<CriarCursoDto, Curso>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InstrutorId, opt => opt.Ignore())
                .ForMember(dest => dest.DataPublicacao, opt => opt.Ignore())
                .ForMember(dest => dest.Aulas, opt => opt.Ignore());

            // Não mapear AtualizarCursoDto para Curso diretamente

            CreateMap<CriarAulaDto, Aula>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CursoId, opt => opt.Ignore())
                .ForMember(dest => dest.Ordem, opt => opt.Ignore());

            // Mapeamento de entidade de domínio para DTO de saída
            CreateMap<Curso, CursoDto>();
            CreateMap<Aula, AulaDto>();
            CreateMap<ConteudoProgramatico, ConteudoProgramaticoDto>().ReverseMap();
        }
    }
}