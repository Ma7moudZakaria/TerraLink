using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.FollowUpCalls.Endpoints;

namespace TerraLink.UseCase.Sales.Features.FollowUpCalls.Specifications;

public sealed class CreateFollowUpCallAddSpec(Guid clientId, CreateFollowUpCallEndpoint.Request request) : IAddSpecification<FollowUpCallEntity>
{
    public FollowUpCallEntity Add()
    {
        return ObjectMapper.Map<CreateFollowUpCallEndpoint.Request, FollowUpCallEntity>(request)
            .Set(entity => entity.Id, Guid.CreateVersion7())
            .Set(entity => entity.ClientId, clientId)
            .Set(entity => entity.CreatedBy, "SYS")
            .Set(entity => entity.CreatedDate, DateTime.UtcNow);
    }
}

public sealed class FollowUpCallDetailsSpec(Guid clientId, Guid id) : ISpecification<FollowUpCallEntity>
{
    public IQueryable<FollowUpCallEntity> Where(IQueryable<FollowUpCallEntity> query)
    {
        return IncludeClient(query)
            .Where(entity => entity.Id == id && entity.ClientId == clientId && !entity.IsDeleted);
    }

    internal static IQueryable<FollowUpCallEntity> IncludeClient(IQueryable<FollowUpCallEntity> query)
    {
        return query.Include(entity => entity.Client);
    }
}

public sealed class FollowUpCallsListSpec(Guid clientId) : ISpecification<FollowUpCallEntity>
{
    public IQueryable<FollowUpCallEntity> Where(IQueryable<FollowUpCallEntity> query)
    {
        return FollowUpCallDetailsSpec.IncludeClient(query)
            .Where(entity => entity.ClientId == clientId && !entity.IsDeleted)
            .OrderByDescending(entity => entity.CallDate);
    }
}

public sealed class SoftDeleteFollowUpCallUpdateSpec : IUpdateSpecification<FollowUpCallEntity>
{
    public Action<UpdateSettersBuilder<FollowUpCallEntity>> Update()
    {
        return setters => setters
            .SetProperty(entity => entity.IsDeleted, true)
            .SetProperty(entity => entity.UpdatedDate, DateTime.UtcNow);
    }
}

public sealed class UpdateFollowUpCallFieldsSpec : IUpdateSpecification<FollowUpCallEntity>
{
    public required DateTime CallDate { get; init; }
    public required string CallerName { get; init; }
    public string? Note { get; init; }
    public required Guid ClientId { get; init; }

    public Action<UpdateSettersBuilder<FollowUpCallEntity>> Update()
    {
        return setters => setters
            .SetProperty(entity => entity.CallDate, CallDate)
            .SetProperty(entity => entity.CallerName, CallerName)
            .SetProperty(entity => entity.Note, Note)
            .SetProperty(entity => entity.ClientId, ClientId)
            .SetProperty(entity => entity.UpdatedDate, DateTime.UtcNow);
    }
}
