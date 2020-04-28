using Prism.Regions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TDSDispatcher.Models;
using TDSDispatcher.Repositories;
using TDSDispatcher.Views;
using TDSDTO.Filter;
using Unity;

namespace TDSDispatcher.Services
{
    class ReferenceService : ISelectable, ISearchAware
    {
        private readonly IUnityContainer container;
        private readonly ITDSRepository repository;

        public ReferenceService(IUnityContainer container, ITDSRepository repository)
        {
            this.container = container;
            this.repository = repository;
        }

        public async Task<IEnumerable> SearchAsync(string referenceName, string fieldName, string text)
        {
            var entityInfo = repository.GetEntityByName(referenceName);
            if (entityInfo != null)
            {
                var mi = repository.GetType()
                    .GetMethod("GetList", new Type[] { typeof(string), typeof(Filter), typeof(CancellationToken) })
                    .MakeGenericMethod(entityInfo.ModelType);
                var task = (Task)mi.Invoke(repository, new object[] 
                {
                    entityInfo.URL,
                    new FilterCondition<FieldOperand, string>(new FieldOperand(fieldName), text, ConditionOperation.Contains),
                    CancellationToken.None 
                });
                await task;
                return (IEnumerable)task.GetType().GetProperty("Result").GetValue(task);
            }
            return null;
        }

        public object Select(string refName, object selectedItem = null, Window owner = null, object filterParameter = null)
        {
            var entityInfo = repository.GetEntityByName(refName);
            if (entityInfo != null)
            {
                return container.Resolve<ElementView>().Select(entityInfo, selectedItem, owner, filterParameter);
            }
            return null;
        }
    }
}
