using System.Collections.Generic;
using TDSDispatcher.Models;
using TDSDTO.Documents;
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
            },
            {
                "Material", new EntityInfo
                {
                    ModelName = nameof(Material),
                    Title = "Материалы",
                    URL = "Materials"
                }
            },
            {
                "Measure", new EntityInfo
                {
                    ModelName = nameof(Measure),
                    Title = "Единицы измерения",
                    URL = "Measures"
                }
            },
            {
                "Order", new EntityInfo
                {
                    ModelName = nameof(Order),
                    Title = "Заявки",
                    URL = "Orders"
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
                    Id = 6,
                    EntityName = "Measure",
                    Title = "Единицы измерения",
                    ParentId = 1
                },
                new MenuItem
                {
                    Id = 7,
                    EntityName = "Material",
                    Title = "Материалы",
                    ParentId = 1
                },
            new MenuItem
            {
                Id = 8,
                Title = "Документы"
            },
                new MenuItem
                {
                    Id = 9,
                    Title = "Заявки",
                    EntityName = "Order",
                    ParentId = 8
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
