using LowCodeHub.QueryableExtensions.Entities;
using LowCodeHub.QueryableExtensions.Specifications;
using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Lookup.Features.LookupItems.Specifications;

public sealed class CreateLookupItemAddSpec(
    Guid id,
    Guid lookupSetId,
    string code,
    Localized? descriptions,
    int sortOrder,
    AttributeDictionary? metadata) : IAddSpecification<LookupItemEntity>
{
    public LookupItemEntity Add() => new()
    {
        Id = id,
        LookupSetId = lookupSetId,
        Code = code,
        Descriptions = descriptions!,
        SortOrder = sortOrder,
        Metadata = metadata,
        IsActive = true
    };
}
