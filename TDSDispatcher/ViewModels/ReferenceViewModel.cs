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
using System.Windows.Input;
using TDSDispatcher.Helpers;
using TDSDispatcher.Models;
using TDSDispatcher.Services;
using TDSDispatcher.Views;
using Unity;

namespace TDSDispatcher.ViewModels
{
    [RegionMemberLifetime(KeepAlive = true)]
    class ReferenceViewModel<T> : BindableBase, INavigationAware
    {
        private readonly ITdsApiService apiService;
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;
        private string refName;

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

        private object currentItem;

        public object CurrentItem
        {
            get => currentItem;
            set => SetProperty(ref currentItem, value);
        }


        #region Commands
        public ICommand AddCommand => new DelegateCommand<Window>(
            x =>
            {
                var ev = container.Resolve<ElementView>();
                ev.Navigate(refName, false, x);
            });

        public ICommand EditCommand => new DelegateCommand<Window>(
            x =>
            {
                var ev = container.Resolve<ElementView>();
                ev.Navigate(refName, true, x);
            });

        public ICommand DeleteCommand => new DelegateCommand(
            () =>
            {

            });
        #endregion

        public ReferenceViewModel(ITdsApiService apiService, IRegionManager regionManager, IUnityContainer container)
        {
            this.apiService = apiService;
            this.regionManager = regionManager;
            this.container = container;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if(navigationContext.Parameters.TryGetValue("MenuItem", out MenuItem menuItem))
            {
                return menuItem.ModelName == refName;
            }
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            if(navigationContext.Parameters.TryGetValue("MenuItem", out MenuItem menuItem))
            {
                Items = new ObservableCollection<T>(await apiService.GetReferenceAsync<T>(menuItem.URL));
                Title = menuItem.Title;
                refName = menuItem.ModelName;
            }
        }
    }
}
