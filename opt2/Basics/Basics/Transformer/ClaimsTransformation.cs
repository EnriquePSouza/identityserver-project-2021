using Microsoft.AspNetCore.Authentication;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Basics.Transformer
{
    public class ClaimsTransformation : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var hasFriendClaim = principal.Claims.Any(x => x.Type == "Friend");
            
            // Sempre que executar uma ação na view ele add a claim,
            // para quando vc não puder armazenar uma claim, basicamente isso.
            if (!hasFriendClaim)
            {
                ((ClaimsIdentity) principal.Identity).AddClaim(new Claim("Friend", "Bad"));
            }

            return Task.FromResult(principal);
        }
    }
}