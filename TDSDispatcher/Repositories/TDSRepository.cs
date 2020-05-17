using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TDSDispatcher.Models;
using TDSDispatcher.Services;
using TDSDTO;
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
                    ModelType = typeof(Employee),
                    Permission = "Reference"
                } 
            },
            {
                "Position", new EntityInfo
                {
                    ModelName = nameof(Position),
                    Title = "Должности",
                    URL = "Positions",
                    ModelType = typeof(Position),
                    Permission = "Position"
                }
            },
            {
                "Counterparty", new EntityInfo
                {
                    ModelName = nameof(Counterparty),
                    Title = "Контрагенты",
                    URL = "Counterparties",
                    ModelType = typeof(Counterparty),
                    Permission = "Reference"
                }
            },
            {
                "User", new EntityInfo
                {
                    ModelName = nameof(User),
                    Title = "Пользователи",
                    URL = "Users",
                    ModelType = typeof(User),
                    Permission = "User"
                }
            },
            {
                "Material", new EntityInfo
                {
                    ModelName = nameof(Material),
                    Title = "Материалы",
                    URL = "Materials",
                    ModelType = typeof(Material),
                    Permission = "Reference"
                }
            },
            {
                "Measure", new EntityInfo
                {
                    ModelName = nameof(Measure),
                    Title = "Единицы измерения",
                    URL = "Measures",
                    ModelType = typeof(Measure),
                    Permission = "Reference"
                }
            },
            {
                "Order", new EntityInfo
                {
                    ModelName = nameof(Order),
                    Title = "Заявки",
                    URL = "Orders",
                    ModelType = typeof(Order),
                    Permission = "Order"
                }
            },
            {
                "GasStation", new EntityInfo
                {
                    ModelName = nameof(GasStation),
                    Title = "АЗС",
                    URL = "GasStations",
                    ModelType = typeof(GasStation),
                    Permission = "Reference"
                }
            },
            {
                "Refuel", new EntityInfo
                {
                    ModelName = nameof(Refuel),
                    Title = "Заправки",
                    URL = "Refuels",
                    ModelType = typeof(Refuel),
                    Permission = "Refuel"
                }
            },
            {
                "CounterpartyRestCorrection", new EntityInfo
                {
                    ModelName = nameof(CounterpartyRestCorrection),
                    Title = "Корректировка остатков поставщика",
                    URL = "CounterpartyRestCorrections",
                    ModelType = typeof(CounterpartyRestCorrection)
                }
            },
            {
                "CounterpartyMaterialRest", new EntityInfo
                {
                    ModelName = nameof(CounterpartyMaterialRest),
                    Title = "Остатки поставщиков",
                    URL = "CounterpartyRests",
                    ModelType = typeof(CounterpartyMaterialRest)
                }
            },
            {
                "Role", new EntityInfo
                {
                    ModelName = nameof(Role),
                    Title = "Роли пользователей",
                    URL = "Roles",
                    ModelType = typeof(Role)
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
                //new MenuItem
                //{
                //    Id = 6,
                //    EntityName = "Measure",
                //    Title = "Единицы измерения",
                //    ParentId = 1
                //},
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
                    Id = 11,
                    Title = "Заправки",
                    EntityName = "Refuel",
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

        private Dictionary<string, ICollection<EntityColumn>> entityColumnsCache = new Dictionary<string, ICollection<EntityColumn>>();

        public TDSRepository(ITdsApiService apiService)
        {
            this.apiService = apiService;
        }

        public async Task<ICollection<CounterpartyMaterialRest>> GetRestsByCounterpartyId(int id)
        {
            var url = GetEntityByName(nameof(CounterpartyMaterialRest)).URL;
            var res = await apiService.GetByCounterpartyId<CounterpartyMaterialRest>(url, id);
            if(String.IsNullOrWhiteSpace(res.Error))
            {
                return res.Result;
            }
            throw new Exception(res.Error);
        }

        public ICollection<EntityInfo> GetEntities()
        {
            return entities.Values;
        }

        public async Task<T> GetEntityByIdAsync<T>(int id)
        {
            var res = await apiService.GetReferenceEntityByIdAsync<T>(GetEntityByName(typeof(T).Name).URL, id);
            if (String.IsNullOrEmpty(res.Error))
            {
                return res.Result;
            }
            throw new Exception(res.Error);
        }

        public EntityInfo GetEntityByName(string name)
        {
            if (entities.TryGetValue(name, out EntityInfo res))
                return res;
            return null;
        }

        public Task<ICollection<T>> GetListAsync<T>(string entityName, CancellationToken token)
        {
            return GetListAsync<T>(entityName, null, token);
        }

        public async Task<ICollection<T>> GetListAsync<T>(string entityName, Filter filter, CancellationToken token)
        {
            var res = await apiService.GetReferenceAsync<T>(entityName, filter, token);
            if (string.IsNullOrWhiteSpace(res.Error))
            {
                return res.Result;
            }

            throw new Exception(res.Error);
        }

        public ICollection<MenuItem> GetMenuItems(ICollection<string> permissions)
        {
            return menuItems
                .Where(x => permissions.Any(p => String.IsNullOrEmpty(x.EntityName) || p.Contains(GetEntityByName(x.EntityName).Permission)))
                .ToList();
        }

        public async Task<bool> MarkUnmarkToDeleteAsync<T>(T entity) where T : BaseModel
        {
            var res = await apiService.MarkUnmarkToDeleteAsync(GetEntityByName(typeof(T).Name).URL, entity.Id);
            if (String.IsNullOrEmpty(res.Error))
                return true;

            throw new Exception(res.Error);
        }

        public async Task<bool> SaveReferenceAsync<T>(T entity)
        {
            var url = GetEntityByName(typeof(T).Name).URL;
            var res = await apiService.SaveReferenceModelAsync<T>(url, entity);
            if(String.IsNullOrEmpty(res.Error))
            {
                return true;
            }
            throw new Exception(res.Error);
        }

        public Task<DateTime> GetLastChangeDate<T>()
        {
            var url = GetEntityByName(typeof(T).Name).URL;
            return apiService.GetLastChangeDate(url);
        }

        public ICollection<EntityColumn> GetEntityDisplayColumns<T>()
        {
            var type = typeof(T);

            if (!entityColumnsCache.TryGetValue(type.FullName, out ICollection<EntityColumn> entityColumns))
            {
                entityColumns = type.GetProperties().Select(
                    x =>
                    {
                        var attr = x.GetCustomAttribute<DisplayFormatAttribute>();
                        if (attr == null || String.IsNullOrEmpty(attr.DisplayName))
                            return null;

                        return new EntityColumn
                        {
                            DisplayName = attr.DisplayName,
                            Name = x.Name
                        };
                    })
                    .Where(x => x != null)
                    .ToList();
                entityColumnsCache[type.FullName] = entityColumns;
            }

            return entityColumns;
        }
    }
}
