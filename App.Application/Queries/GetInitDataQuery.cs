using App.Contracts.Responses;
using Mediator;

namespace App.Application.Queries;

public sealed record GetInitDataQuery(Guid PlayerId) : IQuery<InitDataResponse>;