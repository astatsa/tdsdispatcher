using System.Collections;
using System.Threading.Tasks;

namespace TDSDispatcher.Services
{
    public interface ISearchAware
    {
        Task<IEnumerable> SearchAsync(string referenceName, string fieldName, string text);
    }
}
