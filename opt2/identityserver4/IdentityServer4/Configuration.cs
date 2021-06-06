using IdentityModel;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource> { 
                new ApiResource("ApiOne") //declaração da api
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client_id",
                    ClientSecrets = { new Secret("client_secret".ToSha256()) }, // Esse é apenas um exemplo, o segredo precisa ser complexo, vc pode usar um guid como segredo.
                    
                    AllowedGrantTypes = GrantTypes.ClientCredentials, // Como pegará o access token

                    AllowedScopes = { "ApiOne" } // Diz qual api esse client pode acessar
                }
            };
    }
}
