using Refit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TDSDispatcher.Models;
using TDSDTO;
using TDSDTO.Filter;
using DTO = TDSDTO;

namespace TDSDispatcher.Services
{
    [Headers("Content-type: application/json", "Accept: application/json")]
    interface ITdsApiService
    {
        [Post("/auth")]
        Task<AuthResult> Auth([Body]object authModel, CancellationToken cancellationToken);

        [Get("/{name}")]
        Task<ApiResult<ICollection<T>>> GetReferenceAsync<T>(string name, [Body] Filter filter, CancellationToken cancellationToken);

        [Post("/{name}")]
        Task<DTO.ApiResult<bool>> SaveReferenceModelAsync<T>(string name, [Body] T model);
    }
}
