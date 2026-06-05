using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Models;

namespace TerraLink.UseCase.Asset.Features.Units.Endpoints;

public sealed partial class UpdateUnitEndpoint
{
    public sealed class Validator : IMinimalValidator<Request>
    {
        public IEnumerable<ValidationFailure> Validate(Request request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                yield return new ValidationFailure { PropertyName = nameof(request.Name), ErrorMessage = "Name is required." };
            }

            if (request.BuildingId == Guid.Empty)
            {
                yield return new ValidationFailure { PropertyName = nameof(request.BuildingId), ErrorMessage = "BuildingId is required." };
            }

            if (request.UnitStatusId == Guid.Empty)
            {
                yield return new ValidationFailure { PropertyName = nameof(request.UnitStatusId), ErrorMessage = "UnitStatusId is required." };
            }

            if (request.FloorNumber < 0)
            {
                yield return new ValidationFailure { PropertyName = nameof(request.FloorNumber), ErrorMessage = "FloorNumber cannot be negative." };
            }

            if (request.Area <= 0)
            {
                yield return new ValidationFailure { PropertyName = nameof(request.Area), ErrorMessage = "Area must be greater than zero." };
            }

            if (request.NumberOfRooms <= 0)
            {
                yield return new ValidationFailure { PropertyName = nameof(request.NumberOfRooms), ErrorMessage = "NumberOfRooms must be greater than zero." };
            }

            if (request.NumberOfBatEmployeeooms < 0)
            {
                yield return new ValidationFailure { PropertyName = nameof(request.NumberOfBatEmployeeooms), ErrorMessage = "NumberOfBatEmployeeooms cannot be negative." };
            }

            if (request.UnitTypeId == Guid.Empty)
            {
                yield return new ValidationFailure { PropertyName = nameof(request.UnitTypeId), ErrorMessage = "UnitTypeId is required." };
            }

            if (request.Price <= 0)
            {
                yield return new ValidationFailure { PropertyName = nameof(request.Price), ErrorMessage = "Price must be greater than zero." };
            }

            if (request.FinishingTypeId == Guid.Empty)
            {
                yield return new ValidationFailure { PropertyName = nameof(request.FinishingTypeId), ErrorMessage = "FinishingTypeId is required." };
            }
        }
    }
}
