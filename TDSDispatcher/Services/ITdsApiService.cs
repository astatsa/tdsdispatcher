using Refit;
using System.Threading;
using System.Threading.Tasks;
using TDSDispatcher.Models;

namespace TDSDispatcher.Services
{
    [Headers("Content-type: application/json", "Accept: application/json")]
    interface ITdsApiService
    {
        [Post("/auth")]
        Task<AuthResult> Auth([Body]object authModel, CancellationToken cancellationToken);
    }
}
