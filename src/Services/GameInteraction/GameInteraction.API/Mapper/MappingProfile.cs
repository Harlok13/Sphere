using AutoMapper;
using GameInteraction.Contracts.Data;
using GameInteraction.Domain.Entities;
using GameInteraction.Domain.Entities.PlayerEntity;
using Newtonsoft.Json;

namespace GameInteraction.API.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Player, PlayerDto>()
            .ForMember(dest => dest.Cards, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<List<Card>>(src.Cards)))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId))
            .ForMember(dest => dest.IsLeader, opt => opt.MapFrom(src => src.IsLeader))
            .ForMember(dest => dest.Readiness, opt => opt.MapFrom(src => src.Readiness))
            .ForMember(dest => dest.PlayerName, opt => opt.MapFrom(src => src.PlayerName))
            .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
            .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarUrl))
            .ForMember(dest => dest.Move, opt => opt.MapFrom(src => src.Move))
            .ForMember(dest => dest.Money, opt => opt.MapFrom(src => src.Money))
            .ForMember(dest => dest.InGame, opt => opt.MapFrom(src => src.InGame))
            .ForMember(dest => dest.Online, opt => opt.MapFrom(src => src.Online));
    }
}