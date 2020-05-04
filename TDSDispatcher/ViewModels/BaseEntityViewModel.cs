using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TDSDispatcher.Extensions;
using TDSDispatcher.Helpers;
using TDSDispatcher.Models;
using TDSDispatcher.Repositories;
using TDSDispatcher.Services;

namespace TDSDispatcher.ViewModels
{
    class BaseEntityViewModel<T> : BindableBase, INavigationAware, ICloseRequest, IRegionMemberLifetime where T : new()
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
            set => SetProperty(ref model, value, ModelChanged);
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

        public BaseEntityViewModel(ReferenceService referenceService, ITdsApiService apiService, IDialogService dialogService, ITDSRepository repository)
        {
            this.referenceService = referenceService;
            this.apiService = apiService;
            this.dialogService = dialogService;
            this.repository = repository;
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
                else
                {
                    Model = new T();
                }
            }
        }
        #endregion

        protected virtual async Task<bool> Save()
        {
            bool res = true;
            string error = null;
            try
            {
                var apiRes = await apiService.SaveReferenceModelAsync(entityInfo.URL, Model);
                if (!String.IsNullOrEmpty(apiRes.Error))
                {
                    res = false;
                    error = apiRes.Error;
                }
            }
            catch(Exception ex)
            {
                error = ex.Message;
                res = false;
            }
            if(!res)
            {
                dialogService.ShowMessageBox("Ошибка", $"Произошла ошибка при сохранении изменений!", detail: error);
            }
            return res;
        }

        protected virtual void ModelChanged()
        {

        }

        protected async Task<TEntity> GetEntityByIdAsync<TEntity>(int id)
        {
            if(id != 0)
            {
                try
                {
                     return await repository.GetEntityByIdAsync<TEntity>(id);
                }
                catch(Exception ex)
                {
                    dialogService.ShowMessageBox("Ошибка", $"Произошла ошибка при получении данных!", detail: ex.Message);
                }
                
            }
            return default;
        }

        protected async Task SetEntityByIdAsync<TEntity>(int? id, Action<TEntity> setAction)
        {
            if (!id.HasValue)
                return;

            if (id.Value != 0)
            {
                try
                {
                    setAction(await repository.GetEntityByIdAsync<TEntity>(id.Value));                    
                }
                catch (Exception ex)
                {
                    dialogService.ShowMessageBox("Ошибка", $"Произошла ошибка при получении данных!", detail: ex.Message);
                }
            }
        }


        public event EventHandler<bool> CloseRequest;
        protected EntityInfo entityInfo;
        protected readonly ITdsApiService apiService;
        protected readonly IDialogService dialogService;
        private readonly ITDSRepository repository;
    }
}
