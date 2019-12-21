using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShibExample.Web.Auth
{
    public interface IShibClaim
    {
        ClaimsPrincipal BuildClaimsPrincipal(string username);
    }
}
