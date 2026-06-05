using LowCodeHub.QueryableExtensions.Models;
using Microsoft.AspNetCore.Mvc;

namespace TerraLink.UseCase.IdentityShield.Features.Users.Endpoints
{
    public sealed partial class GetUsersEndpoint
    {
        public sealed class Request : PagedQueryBase
        {
            [FromQuery] public string? SearchTerm { get; set; }
            [FromQuery] public string? UserName { get; set; }
            [FromQuery] public string? Name { get; set; }
            [FromQuery] public string? Email { get; set; }
            [FromQuery] public string? Phone { get; set; }
            [FromQuery] public Guid? RoleId { get; set; }
            [FromQuery] public bool? IsActive { get; set; }
        }
    }
}
