using System;
using System.Collections.Generic;
using TDSDispatcher.Models;
using TDSDTO.References;

namespace TDSDispatcher.Repositories
{
    public class TDSConstRepository : ITDSRepository
    {
        private readonly Dictionary<string, EntityInfo> entities = new Dictionary<string, EntityInfo>
        {
            { 
                "Employee", new EntityInfo
                {
                    ModelName = nameof(Employee),
                    Title = "Сотрудники",
                    URL = "Employees"
                } 
            },
            {
                "Position", new EntityInfo
                {
                    ModelName = nameof(Position),
                    Title = "Должности",
                    URL = "Positions"
                }
            },
            {
                "Counterparty", new EntityInfo
                {
                    ModelName = nameof(Counterparty),
                    Title = "Контрагенты",
                    URL = "Counterparties"
                }
            },
            {
                "User", new EntityInfo
                {
                    ModelName = nameof(User),
                    Title = "Пользователи",
                    URL = "Users"
                }
            }
        };

        private readonly List<MenuItem> menuItems = new List<MenuItem>
        {
            new MenuItem
            {
                Id = 1,
                Title = "Справочники"
            },
                new MenuItem
                {
                    Id = 2,
                    EntityName = "Employee",
                    Title = "Сотрудники",
                    ParentId = 1
                },
                new MenuItem
                {
                    Id = 3,
                    EntityName = "Counterparty",
                    Title = "Контрагенты",
                    ParentId = 1
                },
            new MenuItem
            {
                Id = 4,
                Title = "Администрирование"
            },
                new MenuItem
                {
                    Id = 5,
                    Title = "Пользователи",
                    EntityName = "User",
                    ParentId = 4
                }
        };
        public TDSConstRepository()
        {

        }
        public ICollection<EntityInfo> GetEntities()
        {
            return entities.Values;
        }

        public EntityInfo GetEntityByName(string name)
        {
            if (entities.TryGetValue(name, out EntityInfo res))
                return res;
            return null;
        }

        public ICollection<MenuItem> GetMenuItems()
        {
            return menuItems;
        }
    }
}
