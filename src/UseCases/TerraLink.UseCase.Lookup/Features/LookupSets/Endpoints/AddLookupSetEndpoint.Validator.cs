using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Models;

namespace TerraLink.UseCase.Lookup.Features.LookupSets.Endpoints;

public sealed partial class AddLookupSetEndpoint
{
    public sealed class Validator : IMinimalValidator<Request>
    {
        public IEnumerable<ValidationFailure> Validate(Request request)
        {
            if (string.IsNullOrWhiteSpace(request.Code))
            {
                yield return new ValidationFailure { PropertyName = nameof(request.Code), ErrorMessage = "Code is required." };
            }
        }
    }
}
