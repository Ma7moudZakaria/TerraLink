using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.Installments.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Finance.Features.Installments.Operations;

public sealed partial class CreateInstallmentOperation(
    IRepository<ContractInstallmentEntity> installments,
    IRepository<ContractEntity> contracts)
    : IOperationHandler<CreateInstallmentOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(CreateInstallmentOperation.Request request, CancellationToken ct = default)
    {
        Error? validationError = await InstallmentValidation.ValidateAsync(contracts, request.ContractId, request.Description, request.DueDate, request.Amount, request.AmountText, null, ct);
        if (validationError is not null) return validationError.Value;

        installments.Add(new CreateInstallmentAddSpec(request.ContractId, request.Description, request.DueDate, request.Amount, request.AmountText));

        if (await installments.SaveChangesAsync(ct) > 0) return new Success();
        return Errors.CreateFaild;
    }
}
