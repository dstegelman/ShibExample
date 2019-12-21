using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShibExample.Web.Auth
{
    public class ShibClaim : IShibClaim
    {
        private readonly List<Claim> _userClaims;
        private const string issuer = "https://www.YOURISSUER.edu";
        public ShibClaim()
        {
            this._userClaims = new List<Claim>();
        }
        
        /// <summary>
        /// Formal method for adding teh generic username claim to the identity.  This is where
        /// other claims can be added as well.
        /// </summary>
        /// <param name="username"></param>
        private void BuildUserClaim(string username)
        {
            this._userClaims.Add(new Claim(ClaimTypes.Name, username, ClaimValueTypes.String, issuer));
        }

        /// <summary>
        /// Set permission claims.  You can assign a user
        /// different policy claims based upon their username.
        /// </summary>
        private void SetPermissionClaims(string username)
        {
            // Assigns this user the Admin Claim.  You can map this to a policy in startup.cs
            this._userClaims.Add(new Claim("Admin", "1", ClaimValueTypes.String, issuer));
        }

        /// <summary>
        /// Main entry point for establishing a principal.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public ClaimsPrincipal BuildClaimsPrincipal(string username)
        {
            this.BuildUserClaim(username);
            this.SetPermissionClaims(username);
            var userIdentity = new ClaimsIdentity(this._userClaims, "Passport");
            return new ClaimsPrincipal(userIdentity);
        }
    }
}
