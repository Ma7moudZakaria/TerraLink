using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.IncomingPayments.Specifications;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Operations;

public sealed partial class GetContractInstallmentsPaymentStatusOperation(IRepository<ContractInstallmentEntity> installments, IMapper mapper)
    : IOperationHandler<GetContractInstallmentsPaymentStatusOperation.Request, List<GetContractInstallmentsPaymentStatusOperation.Response>>
{
    public async Task<ErrorOr<List<GetContractInstallmentsPaymentStatusOperation.Response>>> HandleAsync(
        GetContractInstallmentsPaymentStatusOperation.Request request, CancellationToken ct = default)
    {
        IReadOnlyList<ContractInstallmentEntity> result =
            await installments.ListAsync(new InstallmentsPaymentStatusByContractSpec(request.ContractId), ct);

        return result
            .Select(mapper.Map<ContractInstallmentEntity, GetContractInstallmentsPaymentStatusOperation.Response>)
            .ToList();
    }
}
