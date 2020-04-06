using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TDSDispatcher.Services;

namespace TDSDispatcher.ViewModels
{
    class LoginViewModel : BindableBase
    {
        public LoginViewModel(ITdsApiService api)
        {
            apiService = api;
        }

        #region Commands
        public ICommand CloseCommand => new DelegateCommand<Window>(x => x.Close());

        public ICommand LoginCommand => new DelegateCommand(() =>
        {

        });
        #endregion

        private ITdsApiService apiService;
    }
}
