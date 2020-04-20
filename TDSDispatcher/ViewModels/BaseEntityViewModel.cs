using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TDSDispatcher.Helpers;
using TDSDispatcher.Models;
using TDSDispatcher.Services;

namespace TDSDispatcher.ViewModels
{
    class BaseEntityViewModel : BindableBase, INavigationAware, ICloseRequest, IRegionMemberLifetime
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
            () =>
            {
                CloseRequest?.Invoke(this, true);
            });
        #endregion

        public BaseEntityViewModel(ReferenceService referenceService)
        {
            this.referenceService = referenceService;
        }

        #region NavigationAware
        public bool KeepAlive => true;
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
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
            }
        }
        #endregion

        public event EventHandler<bool> CloseRequest;
        protected EntityInfo entityInfo;
    }
}
