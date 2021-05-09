using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Basics.AuthorizationRequirements
{
    // Todo o processo neste arquivo será usado por uma politica de autorização.
    // A politica resolve os prerequisitos e entrega para o sistema se está autorizado ou não.
    // basicamente isso.

    // O requerimento dessa politica de autenticação é o usuário cadastrado ter data de nascimento.
    // Se ele tem data de nascimento ele consegue acessar o methodo bloqueado.
    
    // Request
    public class CustomRequireClaim : IAuthorizationRequirement
    {
        public CustomRequireClaim(string claimType)
        {
            ClaimType = claimType;
        }

        public string ClaimType { get; }
    }

    // Processamento da request
    public class CustomRequireClaimHandler : AuthorizationHandler<CustomRequireClaim>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            CustomRequireClaim requirement)
        {
            // Quando vc cria o usuario essas infos são criadas e armazenadas, e assim vc acessa elas.
            var hasClaim = context.User.Claims.Any(x => x.Type == requirement.ClaimType);
            if (hasClaim)
            {
                context.Succeed(requirement);   
            }

            return Task.CompletedTask;
        }
    }

    public static class AuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequireCustomClaim(
            this AuthorizationPolicyBuilder builder,
            string claymType)
        {
            builder.AddRequirements(new CustomRequireClaim(claymType));
            return builder;
        }
    }
}