using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Input;

namespace TDSDispatcher.ViewModels
{
    [RegionMemberLifetime(KeepAlive = true)]
    class EmployeeViewModel : BindableBase, INavigationAware
    {
        private string title;
        public string Title
        {
            get => $"{title}({(IsEdit ? "Редактирование" : "Новый")})";
            set => SetProperty(ref title, value);
        }

        private bool isEdit;
        public bool IsEdit
        {
            get => isEdit;
            set => SetProperty(ref isEdit, value, () => RaisePropertyChanged(nameof(Title)));
        }

        #region Commands
        public ICommand PositionSelectCommand => new DelegateCommand(
            () =>
            {
                System.Windows.MessageBox.Show("Hello");
            });
        #endregion

        public EmployeeViewModel()
        {
            Title = "Сотрудник";
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
            if (navigationContext.Parameters.TryGetValue<bool>("IsEdit", out bool isEdit))
            {
                IsEdit = isEdit;
            }
        }
    }
}
