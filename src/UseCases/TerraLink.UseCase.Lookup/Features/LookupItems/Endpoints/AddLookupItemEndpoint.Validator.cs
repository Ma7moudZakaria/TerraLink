using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Models;

namespace TerraLink.UseCase.Lookup.Features.LookupItems.Endpoints;

public sealed partial class AddLookupItemEndpoint
{
    public sealed class Validator : IMinimalValidator<Request>
    {
        public IEnumerable<ValidationFailure> Validate(Request request)
        {
            if (request.LookupSetId == Guid.Empty)
            {
                yield return new ValidationFailure { PropertyName = nameof(request.LookupSetId), ErrorMessage = "LookupSetId is required." };
            }

            if (string.IsNullOrWhiteSpace(request.Code))
            {
                yield return new ValidationFailure { PropertyName = nameof(request.Code), ErrorMessage = "Code is required." };
            }
        }
    }
}
