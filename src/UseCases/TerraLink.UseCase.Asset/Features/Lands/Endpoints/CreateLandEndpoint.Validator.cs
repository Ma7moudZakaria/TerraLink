using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Models;

namespace TerraLink.UseCase.Asset.Features.Lands.Endpoints;

public sealed partial class CreateLandEndpoint
{
    public sealed class Validator : IMinimalValidator<Request>
    {
        public IEnumerable<ValidationFailure> Validate(Request request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                yield return new ValidationFailure { PropertyName = nameof(request.Name), ErrorMessage = "Name is required." };
            }

            if (request.GovernorateId == Guid.Empty)
            {
                yield return new ValidationFailure { PropertyName = nameof(request.GovernorateId), ErrorMessage = "GovernorateId is required." };
            }

            if (request.CityId == Guid.Empty)
            {
                yield return new ValidationFailure { PropertyName = nameof(request.CityId), ErrorMessage = "CityId is required." };
            }

            if (request.DistrictId == Guid.Empty)
            {
                yield return new ValidationFailure { PropertyName = nameof(request.DistrictId), ErrorMessage = "DistrictId is required." };
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
