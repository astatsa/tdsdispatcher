using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.RightsManagement;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TDSDispatcher.Helpers;
using TDSDispatcher.Services;
using TDSDispatcher.Views;
using Unity;

namespace TDSDispatcher.ViewModels
{
    class LoginViewModel : BindableBase, ICloseRequest
    {
        private string username;
        public string Username
        {
            get => username;
            set => SetProperty(ref username, value, () => Message = "");
        }

        private bool isLoading;
        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        private string message;
        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        public LoginViewModel(ITdsApiService api, IRegionManager regionManager, SessionContext sessionContext)
        {
            apiService = api;
            this.regionManager = regionManager;
            this.sessionContext = sessionContext;
        }

        #region Commands
        public ICommand CloseCommand => new DelegateCommand<Window>(x => x.Close());

        public ICommand LoginCommand => new DelegateCommand<PasswordBox>(
            async p =>
            {
                IsLoading = true;
                Message = "";
                var cts = new CancellationTokenSource();
                cts.CancelAfter(11000);
                try
                {
                    var result = await apiService.Auth(new
                    {
                        Username,
                        Password = GetPasswordHash(p.Password)
                    }, cts.Token);

                    if (result.Token == null)
                    {
                        Message = "Ошибка авторизации!";
                        return;
                    }

                    sessionContext.Token = result.Token;
                    sessionContext.Employee = result.Employee;

                    new MainWindow(regionManager).Show();
                    CloseRequest?.Invoke(this, true);
                }
                catch(Refit.ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Message = ex.Content ?? ex.Message;
                }
                catch(TaskCanceledException)
                {
                    Message = "Timeout";
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                }
                finally
                {
                    IsLoading = false;
                }
            },
            _ => !IsLoading && Username != null && Username.Length > 0)
            .ObservesProperty(() => IsLoading)
            .ObservesProperty(() => Username);
        #endregion

        private readonly ITdsApiService apiService;
        private readonly IRegionManager regionManager;
        private readonly SessionContext sessionContext;

        public event EventHandler<bool> CloseRequest;

        private string GetPasswordHash(string password)
        {
            using var sha = SHA256.Create();
            return String.Join("", sha.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("x2")));
        }
    }
}
