using AutoMapper;
using UserInteraction.Domain.Entities.PlayerInfoEntity;
using GrpcUserInteractionService;

namespace UserInteraction.API.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PlayerInfo, GrpcGetPlayerInfoResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarUrl))
            .ForMember(dest => dest.PlayerName, opt => opt.MapFrom(src => src.PlayerName))
            .ForMember(dest => dest.Matches, opt => opt.MapFrom(src => src.Matches))
            .ForMember(dest => dest.Loses, opt => opt.MapFrom(src => src.Loses))
            .ForMember(dest => dest.Wins, opt => opt.MapFrom(src => src.Wins))
            .ForMember(dest => dest.Draws, opt => opt.MapFrom(src => src.Draws))
            .ForMember(dest => dest.AllExp, opt => opt.MapFrom(src => src.AllExp))
            .ForMember(dest => dest.CurrentExp, opt => opt.MapFrom(src => src.CurrentExp))
            .ForMember(dest => dest.TargetExp, opt => opt.MapFrom(src => src.TargetExp))
            .ForMember(dest => dest.Money, opt => opt.MapFrom(src => src.Money))
            .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.Likes))
            .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
            .ForMember(dest => dest.Has21, opt => opt.MapFrom(src => src.Has21));
    }
}