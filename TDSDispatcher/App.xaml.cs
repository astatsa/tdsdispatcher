using Microsoft.Extensions.Configuration;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using System;
using System.Net.Http;
using System.Windows;
using TDSDispatcher.Helpers;
using TDSDispatcher.Repositories;
using TDSDispatcher.Services;
using TDSDispatcher.ViewModels;
using TDSDispatcher.ViewModels.Dialogs;
using TDSDispatcher.Views;
using TDSDispatcher.Views.Dialogs;
using TDSDTO;
using TDSDTO.Documents;
using TDSDTO.References;
using Unity;
using Unity.Injection;

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

            containerRegistry.RegisterInstance(new Settings
            {
                ReloadListTimeout = configuration.GetValue<int>("ReloadListTimeoutSec")
            });

            containerRegistry.RegisterSingleton<SessionContext>();
            containerRegistry.RegisterSingleton<PermissionServiceBuilder>();
            containerRegistry.RegisterInstance<ITdsApiService>(Refit.RestService.For<ITdsApiService>(
                    new System.Net.Http.HttpClient(new ApiMessageHandler(new HttpClientHandler(), Container.Resolve<SessionContext>()))
                    {
                        BaseAddress = new Uri(httpSettings.GetValue<string>("Url")),
                        Timeout = TimeSpan.FromMilliseconds(httpSettings.GetValue<int>("Timeout")),
                    }));
            containerRegistry.RegisterSingleton<ITDSRepository, TDSRepository>();

            containerRegistry.RegisterSingleton<ReferenceService>();

            containerRegistry.RegisterForNavigation<EmployeeView>("Employee");
            containerRegistry.RegisterForNavigation<CounterpartyView>("Counterparty");
            containerRegistry.RegisterForNavigation<MeasureView>("Measure");
            containerRegistry.RegisterForNavigation<MaterialView>("Material");
            containerRegistry.RegisterForNavigation<GasStationView>("GasStation");
            containerRegistry.RegisterForNavigation<OrderView>("Order");
            containerRegistry.RegisterForNavigation<RefuelView>("Refuel");
            containerRegistry.RegisterForNavigation<UserView>("User");
            containerRegistry.Register<ElementView>();

            ViewModelLocationProvider.Register<LoginView, LoginViewModel>();
            ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
            ViewModelLocationProvider.Register<EmployeeView, EmployeeViewModel>();
            ViewModelLocationProvider.Register<CounterpartyView, CounterpartyViewModel>();
            ViewModelLocationProvider.Register<MeasureView, MeasureViewModel>();
            ViewModelLocationProvider.Register<MaterialView, MaterialViewModel>();
            ViewModelLocationProvider.Register<GasStationView, GasStationViewModel>();
            ViewModelLocationProvider.Register<OrderView, OrderViewModel>();
            ViewModelLocationProvider.Register<RefuelView, RefuelViewModel>();
            ViewModelLocationProvider.Register<UserView, UserViewModel>();

            containerRegistry.RegisterDialog<MessageBoxView, MessageBoxViewModel>("MessageBox");
            containerRegistry.RegisterDialog<CounterpartyRestView, CounterpartyRestViewModel>();

            RegisterReference<Employee>();
            RegisterReference<Counterparty>();
            RegisterReference<Position>();
            RegisterReference<User>();
            RegisterReference<Measure>();
            RegisterReference<Material>();
            RegisterReference<Order>();
            RegisterReference<GasStation>();
            RegisterReference<Refuel>();
            RegisterReference<Role>();

            containerRegistry.RegisterForNavigation<HomePageView>("HomePage");
            ViewModelLocationProvider.Register<HomePageView, HomePageViewModel>();

            containerRegistry.RegisterForNavigation<HomePageView>();
        }

        private void RegisterReference<TModel>() where TModel : BaseModel
        {
            var cont = Container.GetContainer();
            var modelTypeName = typeof(TModel).Name;
            cont.RegisterType<object, ReferenceViewModel<TModel>>($"{modelTypeName}ListViewModel");
            cont.RegisterType<object, ReferenceView>($"{modelTypeName}List",
                new InjectionConstructor((Func<object>)(() => Container.Resolve<object>($"{modelTypeName}ListViewModel"))));
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            /*ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(
                tv =>
                {
                    Container.Resolve<>
                    return tvm;
                });*/
        }
    }
}
