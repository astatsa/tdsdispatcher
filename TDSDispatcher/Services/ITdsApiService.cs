using Refit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TDSDispatcher.Models;
using DTO = TDSDTO;

namespace TDSDispatcher.Services
{
    [Headers("Content-type: application/json", "Accept: application/json")]
    interface ITdsApiService
    {
        [Post("/auth")]
        Task<AuthResult> Auth([Body]object authModel, CancellationToken cancellationToken);

        [Get("/{name}")]
        Task<ICollection<T>> GetReferenceAsync<T>(string name);

        [Post("/{name}")]
        Task<DTO.ApiResult<bool>> SaveReferenceModelAsync<T>(string name, [Body] T model);
    }
}
