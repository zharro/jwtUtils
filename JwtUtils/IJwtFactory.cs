using System.Threading.Tasks;
using JwtUtils.Models;

namespace JwtUtils
{
    public interface IJwtFactory
    {
        Task<AccessToken> GenerateEncodedToken(string id, string userName);
    }
}
