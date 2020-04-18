using System;
using System.Collections.Generic;
using System.Text;
using TDSDispatcher.Models;

namespace TDSDispatcher.Repositories
{
    public interface ITDSRepository
    {
        ICollection<EntityInfo> GetEntities();
        EntityInfo GetEntityByName(string name);
        ICollection<MenuItem> GetMenuItems();
    }
}
