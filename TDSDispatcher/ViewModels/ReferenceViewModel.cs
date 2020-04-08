using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDSDispatcher.ViewModels
{
    [RegionMemberLifetime(KeepAlive = true)]
    class ReferenceViewModel : BindableBase, INavigationAware
    {
        private string title;
        public string Title
        { 
            get => title; 
            set => SetProperty(ref title, value); 
        }
        public ReferenceViewModel()
        {
            Title = "Test";
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }
    }
}
