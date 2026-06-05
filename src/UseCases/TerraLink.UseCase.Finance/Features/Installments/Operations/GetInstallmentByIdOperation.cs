using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.Installments.Specifications;

namespace TerraLink.UseCase.Finance.Features.Installments.Operations;

public sealed partial class GetInstallmentByIdOperation(IRepository<ContractInstallmentEntity> installments, IMapper mapper)
    : IOperationHandler<GetInstallmentByIdOperation.Request, GetInstallmentByIdOperation.Response>
{
    public async Task<ErrorOr<GetInstallmentByIdOperation.Response>> HandleAsync(GetInstallmentByIdOperation.Request request, CancellationToken ct = default)
    {
        ContractInstallmentEntity? installment = await installments.GetAsync(new InstallmentByIdSpec(request.Id), ct);
        if (installment is null)
            return Error.NotFound("Installment.NotFound", $"Installment with id '{request.Id}' was not found.");

        return mapper.Map<ContractInstallmentEntity, GetInstallmentByIdOperation.Response>(installment);
    }
}
