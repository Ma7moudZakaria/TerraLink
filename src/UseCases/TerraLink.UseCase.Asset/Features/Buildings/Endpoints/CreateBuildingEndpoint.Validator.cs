using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Models;

namespace TerraLink.UseCase.Asset.Features.Buildings.Endpoints;

public sealed partial class CreateBuildingEndpoint
{
    public sealed class Validator : IMinimalValidator<Request>
    {
        public IEnumerable<ValidationFailure> Validate(Request request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                yield return new ValidationFailure { PropertyName = nameof(request.Name), ErrorMessage = "Name is required." };
            }

            if (request.LandId == Guid.Empty)
            {
                yield return new ValidationFailure { PropertyName = nameof(request.LandId), ErrorMessage = "LandId is required." };
            }

            if (request.NumberOfFloors <= 0)
            {
                yield return new ValidationFailure { PropertyName = nameof(request.NumberOfFloors), ErrorMessage = "NumberOfFloors must be greater than zero." };
            }

            if (request.NumberOfUnits < 0)
            {
                yield return new ValidationFailure { PropertyName = nameof(request.NumberOfUnits), ErrorMessage = "NumberOfUnits cannot be negative." };
            }

            if (request.ConstructionYear == default)
            {
                yield return new ValidationFailure { PropertyName = nameof(request.ConstructionYear), ErrorMessage = "ConstructionYear is required." };
            }

            if (request.BuildingStatusId == Guid.Empty)
            {
                yield return new ValidationFailure { PropertyName = nameof(request.BuildingStatusId), ErrorMessage = "BuildingStatusId is required." };
            }

            if (request.Length <= 0)
            {
                yield return new ValidationFailure { PropertyName = nameof(request.Length), ErrorMessage = "Length must be greater than zero." };
            }

            if (request.Width <= 0)
            {
                yield return new ValidationFailure { PropertyName = nameof(request.Width), ErrorMessage = "Width must be greater than zero." };
            }
        }
    }
}
