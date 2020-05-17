using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TDSDispatcher.Models;
using TDSDTO;
using TDSDTO.Filter;

namespace TDSDispatcher.Repositories
{
    public interface ITDSRepository
    {
        ICollection<EntityInfo> GetEntities();
        EntityInfo GetEntityByName(string name);
        ICollection<MenuItem> GetMenuItems(ICollection<string> permissions);

        Task<ICollection<T>> GetListAsync<T>(string entityName, CancellationToken token);
        Task<ICollection<T>> GetListAsync<T>(string entityName, Filter filter, CancellationToken token);

        Task<T> GetEntityByIdAsync<T>(int id);

        Task<bool> MarkUnmarkToDeleteAsync<T>(T entity) where T : BaseModel;

        Task<ICollection<CounterpartyMaterialRest>> GetRestsByCounterpartyId(int id);

        Task<bool> SaveReferenceAsync<T>(T entity);

        Task<DateTime> GetLastChangeDate<T>();

        ICollection<EntityColumn> GetEntityDisplayColumns<T>();
    }
}
