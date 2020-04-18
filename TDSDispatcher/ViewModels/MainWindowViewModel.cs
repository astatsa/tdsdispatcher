using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TDSDispatcher.Helpers;
using TDSDispatcher.Models;
using TDSDispatcher.Repositories;
using TDSDTO.References;

namespace TDSDispatcher.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        public ICommand RefCommand => new DelegateCommand<MenuItemVM>(
            x =>
            {
                regionManager.RequestNavigate(ViewRegions.MainContent, $"{x.EntityName}List",
                    new NavigationParameters
                    {
                        { "EntityInfo", repository.GetEntityByName(x.EntityName) }
                    });
            });

        public ICommand TabCloseCommand => new DelegateCommand<object>(
            x =>
            {
                regionManager.Regions[ViewRegions.MainContent].Remove(x);
            });

        public List<MenuItemVM> MenuItems { get; set; }

        private readonly IRegionManager regionManager;
        private readonly ITDSRepository repository;

        public MainWindowViewModel(IRegionManager regionManager, ITDSRepository repository)
        {
            this.regionManager = regionManager;
            this.repository = repository;

            MenuItems = GetMenuItems(repository.GetMenuItems());
        }

        private List<MenuItemVM> GetMenuItems(ICollection<Models.MenuItem> menuItems, int parentId = 0)
        {
            return menuItems
                .Where(x => x.ParentId == parentId)
                .Select(x => new MenuItemVM
                { 
                    Title = x.Title, 
                    EntityName = x.EntityName, 
                    Childs = GetMenuItems(menuItems, x.Id) 
                })
                .ToList();
        }
    }

    internal class MenuItemVM
    {
        public string Title { get; set; }
        public string EntityName { get; set; }
        public List<MenuItemVM> Childs { get; set; }
}
}
