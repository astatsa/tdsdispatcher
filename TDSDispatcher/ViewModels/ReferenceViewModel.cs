using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TDSDispatcher.Extensions;
using TDSDispatcher.Models;
using TDSDispatcher.Repositories;
using TDSDispatcher.Views;
using TDSDTO.Filter;
using Unity;

namespace TDSDispatcher.ViewModels
{
    [RegionMemberLifetime(KeepAlive = true)]
    class ReferenceViewModel<T> : BindableBase, INavigationAware, ISelectionAware
    {
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;
        private readonly ITDSRepository repository;
        private readonly IDialogService dialogService;
        private EntityInfo entityInfo;
        private CancellationTokenSource cts;
        private Filter filterParameter;

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private ICollection<T> items;

        public ICollection<T> Items
        {
            get => items;
            private set => SetProperty(ref items, value);
        }

        private T currentItem;

        public T CurrentItem
        {
            get => currentItem;
            set => SetProperty(ref currentItem, value);
        }

        private bool selectionMode;

        public event EventHandler<object> Selected;

        public bool SelectionMode
        {
            get => selectionMode;
            set => SetProperty(ref selectionMode, value);
        }

        private bool isLoading;
        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }


        #region Commands
        public ICommand AddCommand => new DelegateCommand<Window>(
            x =>
            {
                var ev = container.Resolve<ElementView>();
                var res = ev.AddOrEdit(entityInfo, false, x);
                if (res.HasValue && res.Value)
                {
                    LoadItems();
                }
            });

        public ICommand EditCommand => new DelegateCommand<Window>(
            x =>
            {
                var ev = container.Resolve<ElementView>();
                var res = ev.AddOrEdit(entityInfo, true, x, CurrentItem);
                if (res.HasValue && res.Value)
                {
                    LoadItems();
                }
            });

        public ICommand DeleteCommand => new DelegateCommand(
            () =>
            {

            });

        public ICommand RowDoubleClickCommand => new DelegateCommand<Window>(
            x =>
            {
                if (SelectionMode)
                {
                    Selected?.Invoke(this, CurrentItem);
                }
                else
                {
                    EditCommand.Execute(x);
                }
            });

        private ICommand keyUpCommand;
        public ICommand KeyUpCommand =>
            keyUpCommand ?? (keyUpCommand = new DelegateCommand<KeyEventArgs>(
                x =>
                {

                }));
        #endregion

        public ReferenceViewModel(IRegionManager regionManager, IUnityContainer container, ITDSRepository repository,
            IDialogService dialogService)
        {
            this.regionManager = regionManager;
            this.container = container;
            this.repository = repository;
            this.dialogService = dialogService;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.TryGetValue("EntityInfo", out EntityInfo entityInfo))
            {
                return this.entityInfo.ModelName == entityInfo.ModelName;
            }
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            if (cts != null && cts.IsCancellationRequested)
            {
                cts.Cancel();
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.TryGetValue("SelectionMode", out bool selectionMode))
            {
                SelectionMode = selectionMode;
                if(SelectionMode && navigationContext.Parameters.TryGetValue("SelectedItem", out T selItem))
                {
                    //TODO:....
                }
            }
            else
            {
                SelectionMode = false;
            }
            navigationContext.Parameters.TryGetValue("FilterParameter", out filterParameter);

            if (navigationContext.Parameters.TryGetValue("EntityInfo", out entityInfo))
            {
                if (entityInfo != null)
                {
                    Title = entityInfo.Title;
                    LoadItems();
                }
            }
        }

        private async void LoadItems()
        {
            cts = new CancellationTokenSource();
            IsLoading = true;
            try
            {
                Items = new ObservableCollection<T>(await repository.GetList<T>(entityInfo.URL, filterParameter, cts.Token));
            }
            catch (TaskCanceledException)
            {

            }
            catch (Refit.ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                dialogService.ShowMessageBox("Ошибка", $"У текущего пользователя нет прав на просмотр справочника \"{Title}\"!");
            }
            catch (Exception ex)
            {
                dialogService.ShowMessageBox("Ошибка", "Произошла ошибка во время запроса данных!", detail: ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
