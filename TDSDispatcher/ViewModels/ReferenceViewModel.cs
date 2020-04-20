using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TDSDispatcher.Helpers;
using TDSDispatcher.Models;
using TDSDispatcher.Repositories;
using TDSDispatcher.Services;
using TDSDispatcher.Views;
using Unity;

namespace TDSDispatcher.ViewModels
{
    [RegionMemberLifetime(KeepAlive = true)]
    class ReferenceViewModel<T> : BindableBase, INavigationAware, ISelectionAware
    {
        private readonly ITdsApiService apiService;
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;
        private readonly ITDSRepository repository;
        private EntityInfo entityInfo;

        private string title;
        public string Title
        { 
            get => title; 
            set => SetProperty(ref title, value); 
        }

        private ICollection<T> items;

        public ICollection<T> Items
        {
            get => items;
            private set => SetProperty(ref items, value);
        }

        private T currentItem;

        public T CurrentItem
        {
            get => currentItem;
            set => SetProperty(ref currentItem, value);
        }

        private bool selectionMode;

        public event EventHandler<object> Selected;

        public bool SelectionMode
        {
            get => selectionMode;
            set => SetProperty(ref selectionMode, value);
        }

        #region Commands
        public ICommand AddCommand => new DelegateCommand<Window>(
            x =>
            {
                var ev = container.Resolve<ElementView>();
                ev.AddOrEdit(entityInfo, false, x);
            });

        public ICommand EditCommand => new DelegateCommand<Window>(
            x =>
            {
                var ev = container.Resolve<ElementView>();
                ev.AddOrEdit(entityInfo, true, x, CurrentItem);
            });

        public ICommand DeleteCommand => new DelegateCommand(
            () =>
            {

            });

        public ICommand RowDoubleClickCommand => new DelegateCommand<Window>(
            x =>
            {
                if(SelectionMode)
                {
                    Selected?.Invoke(this, CurrentItem);
                }
                else
                {
                    EditCommand.Execute(x);
                }
            });

        #endregion

        public ReferenceViewModel(ITdsApiService apiService, IRegionManager regionManager, IUnityContainer container, ITDSRepository repository)
        {
            this.apiService = apiService;
            this.regionManager = regionManager;
            this.container = container;
            this.repository = repository;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if(navigationContext.Parameters.TryGetValue("EntityInfo", out EntityInfo entityInfo))
            {
                return this.entityInfo.ModelName == entityInfo.ModelName;
            }
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            if(navigationContext.Parameters.TryGetValue("EntityInfo", out entityInfo))
            {
                if (entityInfo != null)
                {
                    Items = new ObservableCollection<T>(await apiService.GetReferenceAsync<T>(entityInfo.URL));
                    Title = entityInfo.Title;
                }
            }

            if(navigationContext.Parameters.TryGetValue("SelectionMode", out bool selectionMode))
            {
                SelectionMode = selectionMode;
            }
            else
            {
                SelectionMode = false;
            }
        }
    }
}
