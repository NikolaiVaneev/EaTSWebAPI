using System.Security.Claims;

namespace EaTSWebAPI.Models.Users
{
    public class IdentityResponse
    {
        public ClaimsIdentity claimsIdentity;
        public User user;
    }
}
