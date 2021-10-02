using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NovelQT.Domain.Common.Constants;
using NovelQT.Infra.CrossCutting.Identity.Context;
using NovelQT.Infra.CrossCutting.Identity.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace NovelQT.Infra.CrossCutting.Identity.IdentityServer.Services
{
    public class IdentityProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsFactory;
        private readonly UserManager<AppUser> _userManager;
        private readonly AuthDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityProfileService(
            IUserClaimsPrincipalFactory<AppUser> claimsFactory,
            UserManager<AppUser> userManager,
            AuthDbContext context,
            RoleManager<IdentityRole> roleManager
            )
        {
            _claimsFactory = claimsFactory;
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            if (user == null)
            {
                throw new ArgumentException("");
            }

            var principal = await _claimsFactory.CreateAsync(user);
            var claims = principal.Claims.ToList();
            var roles = await _userManager.GetRolesAsync(user);

            var query = from p in _context.Permissions
                        join c in _context.Commands on p.CommandId equals c.Id
                        join f in _context.Functions on p.FunctionId equals f.Id
                        join r in _roleManager.Roles on p.RoleId equals r.Id
                        where roles.Contains(r.Name)
                        select f.Id + "_" + c.Id;

            var permissions = await query.Distinct().ToListAsync();

            //Add more claims like this
            //claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            //claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            //claims.Add(new Claim(ClaimTypes.Role, string.Join(";", roles)));
            claims.Add(new Claim(IdentityConstant.Claims.Permissions, JsonSerializer.Serialize(permissions)));

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
