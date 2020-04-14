using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TDSDispatcher.Helpers;
using TDSDispatcher.Views;

namespace TDSDispatcher.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        public ICommand RefCommand => new DelegateCommand<string>(
            x =>
            {
                regionManager.RequestNavigate(ViewRegions.MainContent, nameof(ReferenceView), new NavigationParameters($"RefName={x}"));
            });

        public ICommand TabCloseCommand => new DelegateCommand<object>(
            x =>
            {
                regionManager.Regions[ViewRegions.MainContent].Remove(x);
            });

        private IRegionManager regionManager;
        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }
    }
}
