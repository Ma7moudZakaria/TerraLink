using LowCodeHub.QueryableExtensions.Entities;
using LowCodeHub.QueryableExtensions.Specifications;
using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Lookup.Features.LookupSets.Specifications;

public sealed class CreateLookupSetAddSpec(Guid id, string code, Localized? descriptions) : IAddSpecification<LookupSetEntity>
{
    public LookupSetEntity Add() => new()
    {
        Id = id,
        Code = code,
        Descriptions = descriptions!,
        IsActive = true
    };
}
