using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Models;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Endpoints;

public sealed partial class CreateIncomingPaymentEndpoint
{
    public sealed class Validator : IMinimalValidator<Request>
    {
        public IEnumerable<ValidationFailure> Validate(Request request)
        {
            if (request.ContractId == Guid.Empty)
                yield return new ValidationFailure { PropertyName = nameof(request.ContractId), ErrorMessage = "ContractId is required." };
            if (request.ClientId == Guid.Empty)
                yield return new ValidationFailure { PropertyName = nameof(request.ClientId), ErrorMessage = "ClientId is required." };
            if (request.Amount <= 0)
                yield return new ValidationFailure { PropertyName = nameof(request.Amount), ErrorMessage = "Amount must be greater than zero." };
        }
    }
}
