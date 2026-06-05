using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Models;

namespace TerraLink.UseCase.Finance.Features.OutgoingPayments.Endpoints;

public sealed partial class CreateOutgoingPaymentEndpoint
{
    public sealed class Validator : IMinimalValidator<Request>
    {
        public IEnumerable<ValidationFailure> Validate(Request request)
        {
            if (request.ExpenseTypeId == Guid.Empty)
                yield return new ValidationFailure { PropertyName = nameof(request.ExpenseTypeId), ErrorMessage = "ExpenseTypeId is required." };
            if (request.BeneficiaryId == Guid.Empty)
                yield return new ValidationFailure { PropertyName = nameof(request.BeneficiaryId), ErrorMessage = "BeneficiaryId is required." };
            if (request.PaymentMethodId == Guid.Empty)
                yield return new ValidationFailure { PropertyName = nameof(request.PaymentMethodId), ErrorMessage = "PaymentMethodId is required." };
            if (request.Amount <= 0)
                yield return new ValidationFailure { PropertyName = nameof(request.Amount), ErrorMessage = "Amount must be greater than zero." };
        }
    }
}
