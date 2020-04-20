using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TDSDispatcher.Helpers;
using TDSDispatcher.Models;
using TDSDispatcher.Services;

namespace TDSDispatcher.ViewModels
{
    class BaseEntityViewModel<T> : BindableBase, INavigationAware, ICloseRequest, IRegionMemberLifetime where T : class
    {
        #region Properties
        private string title;
        public string Title
        {
            get => $"{title}({(IsEdit ? "Изменение" : "Создание")})";
            set => SetProperty(ref title, value);
        }

        private bool isEdit;
        public bool IsEdit
        {
            get => isEdit;
            set => SetProperty(ref isEdit, value, () => RaisePropertyChanged(nameof(title)));
        }

        private T model;
        public T Model
        {
            get => model;
            set => SetProperty(ref model, value);
        }

        private ReferenceService referenceService;
        public ReferenceService ReferenceService
        {
            get => referenceService;
            private set => SetProperty(ref referenceService, value);
        }
        #endregion

        #region Commands
        public ICommand CancelCommand => new DelegateCommand(
            () =>
            {
                CloseRequest?.Invoke(this, false);
            });

        public ICommand SaveCloseCommand => new DelegateCommand(
            async () =>
            {
                if (await Save())
                {
                    CloseRequest?.Invoke(this, true);
                }
            });
        #endregion

        public BaseEntityViewModel(ReferenceService referenceService, ITdsApiService apiService)
        {
            this.referenceService = referenceService;
            this.apiService = apiService;
        }

        #region NavigationAware
        public bool KeepAlive => true;
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            if(navigationContext.Parameters.TryGetValue<EntityInfo>("EntityInfo", out entityInfo))
            {
                Title = entityInfo.Title;
            }
            if (navigationContext.Parameters.TryGetValue<bool>("IsEdit", out bool isEdit))
            {
                IsEdit = isEdit;
                if(isEdit && navigationContext.Parameters.TryGetValue<T>("Model", out T model))
                {
                    Model = model;
                }
            }
        }
        #endregion

        protected virtual async Task<bool> Save()
        {
            var res = await apiService.SaveReferenceModelAsync(entityInfo.URL, Model);
            return res.Result;
        }

        public event EventHandler<bool> CloseRequest;
        protected EntityInfo entityInfo;
        private readonly ITdsApiService apiService;
    }
}
