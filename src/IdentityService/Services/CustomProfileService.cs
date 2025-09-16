using System;
using System.Security.Claims;
using Duende.IdentityModel;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Services;

public class CustomProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<CustomProfileService> _logger;

    public CustomProfileService(UserManager<ApplicationUser> userManager, ILogger<CustomProfileService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        _logger.LogInformation("UserId is {Subject}", context.Subject);

        var user = await _userManager.GetUserAsync(context.Subject);
        var existingClaims = await _userManager.GetClaimsAsync(user!);
        var claims = new List<Claim>
        {
            new Claim("username", user?.UserName!)
        };

        context.IssuedClaims.AddRange(claims);
        var firstDefaultClaim = existingClaims.FirstOrDefault(x => x.Type == JwtClaimTypes.Name);
        if (firstDefaultClaim is not null)
            context.IssuedClaims.Add(firstDefaultClaim);
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.CompletedTask;
    }
}
