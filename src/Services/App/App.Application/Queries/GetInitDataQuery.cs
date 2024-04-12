using App.Contracts.Data;
using App.Contracts.Responses;
using App.Domain.Shared;
using Mediator;

namespace App.Application.Queries;

public sealed record GetInitDataQuery(
    Guid PlayerId) : IQuery<InitDataResponse>;