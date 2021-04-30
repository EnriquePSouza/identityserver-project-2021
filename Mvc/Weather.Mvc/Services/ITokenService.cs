using System.Threading.Tasks;
using IdentityModel.Client;

namespace Weather.Mvc.Services
{
  public interface ITokenService
  {
    Task<TokenResponse> GetToken(string scope);
  }
}