using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TDSDispatcher.Models;
using TDSDispatcher.Services;
using TDSDTO.Documents;
using TDSDTO.Filter;
using TDSDTO.References;

namespace TDSDispatcher.Repositories
{
    class TDSRepository : ITDSRepository
    {
        private readonly Dictionary<string, EntityInfo> entities = new Dictionary<string, EntityInfo>
        {
            { 
                "Employee", new EntityInfo
                {
                    ModelName = nameof(Employee),
                    Title = "Сотрудники",
                    URL = "Employees",
                    ModelType = typeof(Employee)
                } 
            },
            {
                "Position", new EntityInfo
                {
                    ModelName = nameof(Position),
                    Title = "Должности",
                    URL = "Positions",
                    ModelType = typeof(Position)
                }
            },
            {
                "Counterparty", new EntityInfo
                {
                    ModelName = nameof(Counterparty),
                    Title = "Контрагенты",
                    URL = "Counterparties",
                    ModelType = typeof(Counterparty)
                }
            },
            {
                "User", new EntityInfo
                {
                    ModelName = nameof(User),
                    Title = "Пользователи",
                    URL = "Users",
                    ModelType = typeof(User)
                }
            },
            {
                "Material", new EntityInfo
                {
                    ModelName = nameof(Material),
                    Title = "Материалы",
                    URL = "Materials",
                    ModelType = typeof(Material)
                }
            },
            {
                "Measure", new EntityInfo
                {
                    ModelName = nameof(Measure),
                    Title = "Единицы измерения",
                    URL = "Measures",
                    ModelType = typeof(Measure)
                }
            },
            {
                "Order", new EntityInfo
                {
                    ModelName = nameof(Order),
                    Title = "Заявки",
                    URL = "Orders",
                    ModelType = typeof(Order)
                }
            },
            {
                "GasStation", new EntityInfo
                {
                    ModelName = nameof(GasStation),
                    Title = "АЗС",
                    URL = "GasStations",
                    ModelType = typeof(GasStation)
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
                    Id = 10,
                    EntityName = "GasStation",
                    Title = "АЗС",
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
        private readonly ITdsApiService apiService;

        public TDSRepository(ITdsApiService apiService)
        {
            this.apiService = apiService;
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

        public async Task<ICollection<T>> GetList<T>(string entityName, CancellationToken token)
        {
            var res = await apiService.GetReferenceAsync<T>(entityName, token);
            if(string.IsNullOrWhiteSpace(res.Error))
            {
                return res.Result;
            }

            throw new Exception(res.Error);
        }

        public async Task<ICollection<T>> GetList<T>(string entityName, Filter filter, CancellationToken token)
        {
            var data = await GetList<T>(entityName, token);
            if (filter != null)
            {
                return data.Where(filter.GetPredicate<T>()).ToList();
            }
            return data;
        }

        public ICollection<MenuItem> GetMenuItems()
        {
            return menuItems;
        }
    }
}
