﻿using Prism.Commands;
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
        public ICommand RefCommand => new DelegateCommand(
            () =>
            {
                regionManager.RequestNavigate(ViewRegions.MainContent, nameof(ReferenceView), x => 
                { 
                });
            });

        private IRegionManager regionManager;
        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }
    }
}
