using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TDSDispatcher.Helpers;
using TDSDispatcher.Models;
using TDSDispatcher.Views;
using TDSDTO.References;

namespace TDSDispatcher.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        public ICommand RefCommand => new DelegateCommand<MenuItem>(
            x =>
            {
                regionManager.RequestNavigate(ViewRegions.MainContent, $"{x.ModelName}List", 
                    new NavigationParameters
                    {
                        { "MenuItem", x }
                    });
            });

        public ICommand TabCloseCommand => new DelegateCommand<object>(
            x =>
            {
                regionManager.Regions[ViewRegions.MainContent].Remove(x);
            });

        public List<MenuItem> MenuItems { get; set; }

        private IRegionManager regionManager;
        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            MenuItems = new List<MenuItem>
            {
                new MenuItem
                {
                    Title = "Справочники",
                    Childs = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Title = "Сотрудники",
                            ModelName = nameof(Employee),
                            URL = "Employees"
                        },
                        new MenuItem
                        {
                            Title = "Контрагенты",
                            ModelName = nameof(Counterparty),
                            URL = "Counterparties"
                        }
                    }
                }
            };
        }
    }
}
