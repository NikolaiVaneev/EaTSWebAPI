using Microsoft.AspNetCore.Authorization;
using static EaTSWebAPI.WC;

namespace EaTSWebAPI.Service.Extensions
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params UserRole[] roles) : base()
        {
            Roles = string.Join(",", Enum.GetNames(typeof(UserRole)));
        }
    }
}
