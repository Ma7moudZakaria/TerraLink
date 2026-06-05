using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.Installments.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Finance.Features.Installments.Operations;

public sealed partial class UpdateInstallmentOperation(
    IRepository<ContractInstallmentEntity> installments,
    IRepository<ContractEntity> contracts)
    : IOperationHandler<UpdateInstallmentOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(UpdateInstallmentOperation.Request request, CancellationToken ct = default)
    {
        Error? validationError = await InstallmentValidation.ValidateAsync(contracts, request.ContractId, request.Description, request.DueDate, request.Amount, request.AmountText, request.Id, ct);
        if (validationError is not null) return validationError.Value;

        var update = new UpdateInstallmentFieldsSpec
        {
            ContractId = request.ContractId,
            Description = request.Description,
            DueDate = request.DueDate,
            Amount = request.Amount,
            AmountText = request.AmountText
        };

        if (await installments.UpdateAsync(new InstallmentByIdSpec(request.Id), update, ct) > 0) return new Success();
        return Errors.UpdateFaild;
    }
}
