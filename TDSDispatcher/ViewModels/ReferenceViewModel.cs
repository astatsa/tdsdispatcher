using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TDSDispatcher.Extensions;
using TDSDispatcher.Models;
using TDSDispatcher.Repositories;
using TDSDispatcher.Views;
using TDSDTO;
using TDSDTO.Filter;
using Unity;

namespace TDSDispatcher.ViewModels
{
    [RegionMemberLifetime(KeepAlive = true)]
    class ReferenceViewModel<T> : BindableBase, INavigationAware, ISelectionAware where T : BaseModel
    {
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;
        private readonly ITDSRepository repository;
        private readonly IDialogService dialogService;
        private readonly Settings settings;
        private EntityInfo entityInfo;
        private CancellationTokenSource cts;
        private Filter filterParameter;
        private DateTime lastUpdateDate;

        public bool IsReferenceList => true;
        public bool IsClosable => true;

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
            set
            {
                //Из-за того что Equals сравнивает Id, свойство не обновляется
                currentItem = value;
                RaisePropertyChanged();
            }
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
                    LoadItems(CurrentItem);
                }
            });

        public ICommand DeleteCommand => new DelegateCommand(
            () =>
            {
                if(CurrentItem != null)
                {
                    dialogService.ShowMessageBox("Пометка на удаление", 
                        (CurrentItem.IsDeleted ? "У выбранного элемента будет снята пометка на удаление!" 
                            : "Выбранный элемент будет помечен на удаление!") + " Продолжить?",
                        new ButtonResult[] { ButtonResult.No, ButtonResult.Yes },
                        callback: async x =>
                        {
                            if (x.Result == ButtonResult.No) return;

                            if (await repository.MarkUnmarkToDeleteAsync(CurrentItem))
                            {
                                LoadItems(CurrentItem);
                            }
                        });
                }
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

        public ICommand RefreshCommand => new DelegateCommand<Window>(
            x =>
            {
                LoadItems();
            });

        private ICommand keyUpCommand;
        public ICommand KeyUpCommand =>
            keyUpCommand ?? (keyUpCommand = new DelegateCommand<KeyEventArgs>(
                x =>
                {

                }));
        #endregion

        public ReferenceViewModel(IRegionManager regionManager, IUnityContainer container, ITDSRepository repository,
            IDialogService dialogService, Settings settings)
        {
            this.regionManager = regionManager;
            this.container = container;
            this.repository = repository;
            this.dialogService = dialogService;
            this.settings = settings;

            if(settings.ReloadListTimeout > 0)
                StartRefresher();
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
            T selItem = default;
            if (navigationContext.Parameters.TryGetValue("SelectionMode", out bool selectionMode))
            {
                SelectionMode = selectionMode;
                if(SelectionMode)
                    navigationContext.Parameters.TryGetValue("SelectedItem", out selItem);
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
                    LoadItems(selItem);
                }
            }
        }

        private async void LoadItems(T selItem = null)
        {
            cts = new CancellationTokenSource();
            IsLoading = true;
            try
            {
                Items = await repository.GetListAsync<T>(entityInfo.URL, filterParameter, cts.Token);
                if(selItem != null)
                {
                    CurrentItem = Items.FirstOrDefault(x => x.Equals(selItem));
                }
                lastUpdateDate = DateTime.Now;
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

        private async void StartRefresher()
        {
            while (true)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(settings.ReloadListTimeout));
                    if (!IsLoading)
                    {
                        var res = await repository.GetLastChangeDate<T>();
                        if (res > lastUpdateDate)
                        {
                            LoadItems();
                        }
                    }
                }
                catch(ApiException)
                {
                }
                catch
                {
                }
            }
        }
    }
}
