using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using TDSDispatcher.Helpers;
using TDSDispatcher.Services;

namespace TDSDispatcher.ViewModels
{
    [RegionMemberLifetime(KeepAlive = true)]
    class ReferenceViewModel : BindableBase, INavigationAware
    {
        private readonly ITdsApiService apiService;
        private readonly IRegionManager regionManager;
        private string refName;

        private string title;
        public string Title
        { 
            get => title; 
            set => SetProperty(ref title, value); 
        }

        private ICollection<object> items;

        public ICollection<object> Items
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
        public ICommand AddCommand => new DelegateCommand(
            () =>
            {
                regionManager.RequestNavigate(ViewRegions.MainContent, refName);
            });

        public ICommand EditCommand => new DelegateCommand(
            () =>
            {

            });

        public ICommand DeleteCommand => new DelegateCommand(
            () =>
            {

            });
        #endregion

        public ReferenceViewModel(ITdsApiService apiService, IRegionManager regionManager)
        {
            this.apiService = apiService;
            this.regionManager = regionManager;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            if(navigationContext.Parameters.TryGetValue("RefName", out refName))
            {
                Items = new ObservableCollection<object>(await apiService.GetReferenceAsync(refName));
                Title = refName;
            }
        }
    }
}
