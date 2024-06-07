using System.Transactions;
using Core.DomainResults;
using Core.Extensions;
using Core.Shared;
using GameInteraction.Domain.Entities.RoomEntity;
using Grpc.Core;
using GrpcGameInteractionService;

namespace GameInteraction.API.Services;

public partial class GameInteractionService
{
    public override async Task<GrpcCreateRoomResponse> CreateRoom(GrpcCreateRoomRequest request, ServerCallContext context)
    {
        CancellationToken cT = context.CancellationToken;
        // string? connectionId = request.ConnectionId;
        RoomRequest roomRequest = request.CreateRoomRequest.RoomRequest;
        
        var room = Room.Create(
            id: Guid.NewGuid(),
            roomName: roomRequest.RoomName,
            roomSize: roomRequest.RoomSize,
            startBid: roomRequest.StartBid,
            minBid: roomRequest.MinBid,
            maxBid: roomRequest.MaxBid,
            avatarUrl: roomRequest.AvatarUrl,
            lowerStartMoneyBound: request.CreateRoomRequest.LowerBound,
            upperStartMoneyBound: request.CreateRoomRequest.UpperBound);
        
        await _unitOfWork.RoomRepository.AddAsync(room, cT);
        
        bool success = await _unitOfWork.SaveChangesAsync(cT);

        // return new GrpcCreateRoomResponse { Success = success };

        var grpcJoinToRoomRequest = new GrpcJoinToRoomRequest {
            RoomId = room.Id.ToString(),
            PlayerId = request.CreateRoomRequest.PlayerId,
            SelectedStartMoney = request.CreateRoomRequest.SelectedStartMoney,
            ConnectionId = connectionId };
        
        _logger.LogInformation("User {@UserId}: Invoking gRPC method {@MethodName} with arguments {@Arguments}.",
            request.CreateRoomRequest.PlayerId,
            nameof(JointToRoom),
            grpcJoinToRoomRequest);
        
        GrpcJoinToRoomResponse grpcJoinToRoomResponse = await JointToRoom(grpcJoinToRoomRequest, context);
        
        // return new GrpcCreateRoomResponse { Success = grpcJoinToRoomResponse.Success };
        // return await _mediator.Send(new JoinToRoomCommand(joinToRoomRequest, connectionId), cT);
    }
    
    public override async Task<GrpcJoinToRoomResponse> JointToRoom(GrpcJoinToRoomRequest request, ServerCallContext context)
    {
        // command.Request.Deconstruct(
        //     out Guid roomId, out Guid playerId, out int selectedStartMoney);
        // var connectionId = command.ConnectionId;
        CancellationToken cT = context.CancellationToken;
        Guid playerId = Guid.Parse(request.PlayerId);
        string? connectionId = request.ConnectionId;

        using var scope = new TransactionScope();
        
        try
        {
            var roomResult = await _unitOfWork.RoomRepository.GetByIdAsync(Guid.Parse(request.RoomId), cT);
            if (!roomResult.TryFromResult(out Room? room, out var roomErrors))
            {
                return new GrpcJoinToRoomResponse {
                    Success = await SendSomethingWentWrongNotification(roomErrors, connectionId, cT) };
            }

            var canPlayerJoinResult = room!.CanPlayerJoin(playerId);
            if (canPlayerJoinResult is DomainFailure canPlayerJoinFailure)
            {
                await _publisher.Publish(new ClientNotificationEvent(
                        NotificationText: canPlayerJoinFailure.Reason,
                        TargetConnectionId: connectionId),
                    cT);

                return false;
            }
            
            
            
            var playerInfoResult = await _unitOfWork.PlayerInfoRepository.GetPlayerInfoByIdAsync(playerId, cT);
            if (!playerInfoResult.TryFromResult(out PlayerInfo? playerInfo, out var playerInfoErrors))
            {
                return await SendSomethingWentWrongNotification(playerInfoErrors, connectionId, cT);
            }
            
            var joinToRoomResult = playerInfo!.JoinToRoom(selectedStartMoney);
            if (!joinToRoomResult.TryFromResult(out PlayerInfo.JoinToRoomDto? data, out var joinToRoomErrors))
            {
                return await SendSomethingWentWrongNotification(joinToRoomErrors, connectionId, cT);
            }
            
            
            var joinResult = room.JoinToRoom(
                playerId: playerId,
                playerName: data!.PlayerName, 
                data.Money,
                connectionId: connectionId);
            if (joinResult is DomainFailure joinFailure)
            {
                await _publisher.Publish(new UserNotificationEvent(
                        NotificationText: joinFailure.Reason,
                        TargetId: playerId),
                    cT);

                return false;
            }

            var saveChangesResult = await _unitOfWork.SaveChangesAsync(cT);
            if (!saveChangesResult)
            {
                return await SendSomethingWentWrongNotification(null, connectionId, cT);
            }

            await _publisher.Publish(new UserNavigateEvent(
                    TargetId: playerId,
                    Navigate: NavigateEnum.Room),
                cT);
            scope.Complete();
            
            return saveChangesResult;
        }
        catch (Exception ex)
        {
        }

        throw new Exception();
    }
    
    private async ValueTask<bool> SendSomethingWentWrongNotification(
        IEnumerable<Error>? errors,
        string targetConnectionId,
        CancellationToken cT)
    {
        if (errors is not null)
            foreach (var error in errors) _logger.LogError(error.Message);
            
        await _publisher.Publish(new ClientNotificationEvent(
                NotificationText: NotificationMessages.SomethingWentWrong(),
                TargetConnectionId: targetConnectionId),
            cT);
            
        return false;
    }
}