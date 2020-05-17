using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Exp = System.Linq.Expressions.Expression;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using TDSDispatcher.Extensions;
using TDSDispatcher.Models;
using TDSDispatcher.Repositories;
using TDSDispatcher.Services;
using TDSDispatcher.Views;
using TDSDTO;
using TDSDTO.Filter;
using Unity;
using System.Security.Cryptography.Xml;

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
        private readonly PermissionServiceBuilder permissionServiceBuilder;
        private EntityInfo entityInfo;
        private CancellationTokenSource cts;
        private Filter filterParameter;
        private DateTime lastUpdateDate;

        public bool IsReferenceList => true;
        public bool IsClosable => true;

        private PermissionService permissionService;
        public PermissionService PermissionService 
        { 
            get => permissionService; 
            private set => SetProperty(ref permissionService, value);
        }

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
            private set => SetProperty(ref items, value, 
                () =>
                {
                    var filterPredicate = ItemsView?.Filter;

                    ItemsView = CollectionViewSource.GetDefaultView(items);
                    ItemsView.Filter = filterPredicate;
                });
        }

        private ICollectionView itemsView;
        public ICollectionView ItemsView
        {
            get => itemsView;
            set => SetProperty(ref itemsView, value);
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

        private string filterText;
        public string FilterText
        {
            get => filterText;
            set => SetProperty(ref filterText, value);
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
            }, 
            x => PermissionService?.HasPermission(EntityOperations.Edit) ?? true)
            .ObservesProperty(() => PermissionService);

        public ICommand EditCommand => new DelegateCommand<Window>(
            x =>
            {
                var ev = container.Resolve<ElementView>();
                var res = ev.AddOrEdit(entityInfo, true, x, CurrentItem);
                if (res.HasValue && res.Value)
                {
                    LoadItems(CurrentItem);
                }
            },
            x => PermissionService?.HasPermission(EntityOperations.Edit) ?? true)
            .ObservesProperty(() => PermissionService);

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
            },
            () => PermissionService?.HasPermission(EntityOperations.Edit) ?? true)
            .ObservesProperty(() => PermissionService);

        public ICommand RowDoubleClickCommand => new DelegateCommand<Window>(
            x =>
            {
                if (SelectionMode)
                {
                    Selected?.Invoke(this, CurrentItem);
                }
                else
                {
                    if(EditCommand.CanExecute(x))
                        EditCommand.Execute(x);
                }
            });

        public ICommand RefreshCommand => new DelegateCommand<Window>(
            x =>
            {
                LoadItems();
            });

        private ICommand textInputCommand;
        public ICommand TextInputCommand =>
            textInputCommand ?? (textInputCommand = new DelegateCommand<TextCompositionEventArgs>(
                x =>
                {
                    if (!(x.OriginalSource is DataGridCell cell)
                        || !(cell.Column is DataGridBoundColumn column) 
                        || column.Binding == null
                        || !(column.Binding is Binding binding))
                        return;

                    var searchPlaces = repository.GetEntityDisplayColumns<T>();
                    var columnName = binding.Path.Path;

                    dialogService.ShowDialog("QuickSearch",
                        new DialogParameters
                        {
                            { "SearchText", x.Text },
                            { "SearchPlaces", searchPlaces },
                            { "CurrentSearchPlace", searchPlaces.FirstOrDefault(s => s.Name == columnName) }
                        },
                        result =>
                        {
                            if (result == null || result.Result != ButtonResult.OK
                                || !result.Parameters.TryGetValue("SearchPlace", out EntityColumn entityColumn)
                                || !result.Parameters.TryGetValue("SearchText", out string searchText))
                                return;

                            try
                            {
                                var param = Exp.Parameter(typeof(T));
                                var getProperty = Exp.Lambda<Func<T, object>>(
                                    Exp.Convert(
                                        Exp.Property(
                                            param,
                                            entityColumn.Name),
                                        typeof(object)),
                                    param).Compile();

                                ItemsView.Filter =
                                    x =>
                                    {
                                        var value = getProperty((T)x);
                                        if (value == null)
                                            return false;

                                        return value.ToString().Contains(searchText, StringComparison.OrdinalIgnoreCase);
                                    };

                                FilterText = $"{entityColumn.DisplayName} содержит \"{searchText}\"";
                            }
                            catch(Exception ex)
                            {
                                dialogService.ShowMessageBox("Ошибка", "Ошибка добавления фильтра!", detail: ex.ToString());
                            }
                        });
                }));

        private ICommand clearFilterCommand;
        public ICommand ClearFilterCommand =>
            clearFilterCommand ?? (clearFilterCommand = new DelegateCommand(
                () =>
                {
                    if(ItemsView != null)
                    {
                        ItemsView.Filter = null;
                    }
                    FilterText = String.Empty;
                }));
        #endregion

        public ReferenceViewModel(IRegionManager regionManager, IUnityContainer container, ITDSRepository repository,
            IDialogService dialogService, Settings settings, PermissionServiceBuilder permissionServiceBuilder)
        {
            this.regionManager = regionManager;
            this.container = container;
            this.repository = repository;
            this.dialogService = dialogService;
            this.settings = settings;
            this.permissionServiceBuilder = permissionServiceBuilder;

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
                    PermissionService = permissionServiceBuilder.Build(entityInfo);
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
