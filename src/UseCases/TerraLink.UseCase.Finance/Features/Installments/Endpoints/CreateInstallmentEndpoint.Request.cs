namespace TerraLink.UseCase.Finance.Features.Installments.Endpoints;

public sealed partial class CreateInstallmentEndpoint
{
    public sealed record Request(Guid ContractId, string Description, DateTime DueDate, decimal Amount, string AmountText);
}
