using Microsoft.Extensions.Configuration;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using TDSDispatcher.Helpers;
using TDSDispatcher.Services;
using TDSDispatcher.ViewModels;
using TDSDispatcher.Views;

namespace TDSDispatcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<Views.LoginView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<SessionContext>();
            containerRegistry.RegisterInstance<ITdsApiService>(Refit.RestService.For<ITdsApiService>(
                    new System.Net.Http.HttpClient(new ApiMessageHandler(new HttpClientHandler(), Container.Resolve<SessionContext>()))
                    {
                        BaseAddress = new Uri(ConfigurationManager.AppSettings.Get("TDSServerUrl")),
                        Timeout = TimeSpan.FromMilliseconds(ConfigurationManager.AppSettings.Get("HTTP")),
                    }));

            ViewModelLocationProvider.Register<LoginView, LoginViewModel>();
        }

        public override void Initialize()
        {
            var b = new ConfigurationBuilder();
            base.Initialize();
        }
    }
}
