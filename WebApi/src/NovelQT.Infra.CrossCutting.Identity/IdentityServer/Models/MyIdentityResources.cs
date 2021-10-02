using Duende.IdentityServer.Models;
using NovelQT.Domain.Common.Constants;

namespace NovelQT.Infra.CrossCutting.Identity.IdentityServer.Models
{
    public static class MyIdentityResources
    {
        public class Roles : IdentityResource
        {
            public Roles()
            {
                base.Name = "roles";
                base.DisplayName = "User role(s)";
                base.UserClaims.Add("role");
            }
        }

        public class Permissions : IdentityResource
        {
            public Permissions()
            {
                base.Name = IdentityConstant.Claims.Permissions;
                base.DisplayName = "User permissions(s)";
                base.UserClaims.Add("permission");
            }
        }
    }
}
