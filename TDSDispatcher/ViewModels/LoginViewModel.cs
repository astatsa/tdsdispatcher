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

        public LoginViewModel(ITdsApiService api, IRegionManager regionManager)
        {
            apiService = api;
            this.regionManager = regionManager;
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


                    new MainWindow(regionManager).Show();
                    CloseRequest?.Invoke(this, EventArgs.Empty);
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

        public event EventHandler CloseRequest;

        private string GetPasswordHash(string password)
        {
            using var sha = SHA256.Create();
            return String.Join("", sha.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("x2")));
        }
    }
}
