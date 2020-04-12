using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TDSDispatcher.ViewModels
{
    [RegionMemberLifetime(KeepAlive = true)]
    class EmployeeViewModel : BindableBase, INavigationAware
    {
        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Title = "Сотрудник(Новый)";
        }
    }
}
