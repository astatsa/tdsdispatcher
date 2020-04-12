using Microsoft.Extensions.Configuration;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
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
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Local.json", true)
                .Build();
            var httpSettings = configuration.GetSection("HTTPSettings");

            containerRegistry.RegisterSingleton<SessionContext>();
            containerRegistry.RegisterInstance<ITdsApiService>(Refit.RestService.For<ITdsApiService>(
                    new System.Net.Http.HttpClient(new ApiMessageHandler(new HttpClientHandler(), Container.Resolve<SessionContext>()))
                    {
                        BaseAddress = new Uri(httpSettings.GetValue<string>("Url")),
                        Timeout = TimeSpan.FromMilliseconds(httpSettings.GetValue<int>("Timeout")),
                    }));

            containerRegistry.RegisterForNavigation<ReferenceView>();
            containerRegistry.RegisterForNavigation<EmployeeView>("Employees");

            ViewModelLocationProvider.Register<LoginView, LoginViewModel>();
            ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
            ViewModelLocationProvider.Register<ReferenceView, ReferenceViewModel>();
        }
    }
}
