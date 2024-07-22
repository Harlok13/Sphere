using App.Contracts.Data;
using App.Contracts.Identity.Requests;
using App.Contracts.Identity.Responses;
using AutoMapper;
using GrpcIdentityClient;
using GrpcUserInteractionClient;

namespace App.API.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterRequest, GrpcRegisterRequest>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.PasswordConfirm, opt => opt.MapFrom(src => src.PasswordConfirm))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));

        CreateMap<AuthenticateRequest, GrpcAuthenticateRequest>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

        CreateMap<RefreshTokenRequest, GrpcRefreshTokenRequest>()
            .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken))
            .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.AccessToken));

        CreateMap<GrpcGetPlayerInfoResponse, PlayerInfoResponse>()
            .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarUrl))
            .ForMember(dest => dest.Draws, opt => opt.MapFrom(src => src.Draws))
            .ForMember(dest => dest.Has21, opt => opt.MapFrom(src => src.Has21))
            .ForMember(dest => dest.Loses, opt => opt.MapFrom(src => src.Loses))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
            .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.Likes))
            .ForMember(dest => dest.Matches, opt => opt.MapFrom(src => src.Matches))
            .ForMember(dest => dest.Money, opt => opt.MapFrom(src => src.Money))
            .ForMember(dest => dest.Wins, opt => opt.MapFrom(src => src.Wins))
            .ForMember(dest => dest.AllExp, opt => opt.MapFrom(src => src.AllExp))
            .ForMember(dest => dest.CurrentExp, opt => opt.MapFrom(src => src.CurrentExp))
            .ForMember(dest => dest.PlayerName, opt => opt.MapFrom(src => src.PlayerName))
            .ForMember(dest => dest.TargetExp, opt => opt.MapFrom(src => src.TargetExp));

        CreateMap<GrpcGetPlayerInfoResponse, PlayerInfoDto>();


        CreateMap<PlayerInfoResponse, AuthenticateResponse>()
            .ForMember(dest => dest.PlayerInfo, opt => opt.Ignore());

        CreateMap<GrpcAuthenticateResponse, AuthenticateResponse>()
            .ForMember(dest => dest.Email, opt => opt.Ignore())
            .ForMember(dest => dest.PlayerName, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
            .ForMember(dest => dest.Token, opt => opt.Ignore())
            .ForMember(dest => dest.PlayerId, opt => opt.Ignore());
    }
}