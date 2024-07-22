using AutoMapper;
using GameInteraction.Application.Repositories.UnitOfWork;

namespace GameInteraction.API.Services;

public partial class GameInteractionService : global::GrpcGameInteractionService.GameInteraction.GameInteractionBase
{
    private readonly ILogger<GameInteractionService> _logger;
    private readonly IGameInteractionUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GameInteractionService(
        ILogger<GameInteractionService> logger,
        IGameInteractionUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
}