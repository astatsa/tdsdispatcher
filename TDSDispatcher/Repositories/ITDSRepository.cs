using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TDSDispatcher.Models;
using TDSDTO.Filter;

namespace TDSDispatcher.Repositories
{
    public interface ITDSRepository
    {
        ICollection<EntityInfo> GetEntities();
        EntityInfo GetEntityByName(string name);
        ICollection<MenuItem> GetMenuItems();

        Task<ICollection<T>> GetList<T>(string entityName, CancellationToken token);
        Task<ICollection<T>> GetList<T>(string entityName, Filter filter, CancellationToken token);
    }
}
