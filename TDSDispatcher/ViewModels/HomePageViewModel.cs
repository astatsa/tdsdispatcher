﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using TDSDispatcher.Helpers;
using TDSDispatcher.Repositories;

namespace TDSDispatcher.ViewModels
{
    [RegionMemberLifetime(KeepAlive = true)]
    class HomePageViewModel : BindableBase, INavigationAware
    {
        private readonly ITDSRepository repository;
        private readonly IRegionManager regionManager;

        public bool IsHomePage => true;
        public bool IsClosable => false;

        public Geometry TabPicture => Geometry.Parse(@"M487.083,225.514l-75.08-75.08V63.704c0-15.682-12.708-28.391-28.413-28.391c-15.669,0-28.377,12.709-28.377,28.391
				v29.941L299.31,37.74c-27.639-27.624-75.694-27.575-103.27,0.05L8.312,225.514c-11.082,11.104-11.082,29.071,0,40.158
				c11.087,11.101,29.089,11.101,40.172,0l187.71-187.729c6.115-6.083,16.893-6.083,22.976-0.018l187.742,187.747
				c5.567,5.551,12.825,8.312,20.081,8.312c7.271,0,14.541-2.764,20.091-8.312C498.17,254.586,498.17,236.619,487.083,225.514z M257.561,131.836c-5.454-5.451-14.285-5.451-19.723,0L72.712,296.913c-2.607,2.606-4.085,6.164-4.085,9.877v120.401
				c0,28.253,22.908,51.16,51.16,51.16h81.754v-126.61h92.299v126.61h81.755c28.251,0,51.159-22.907,51.159-51.159V306.79
				c0-3.713-1.465-7.271-4.085-9.877L257.561,131.836z");

        public List<MenuItemVM> MenuItems { get; set; }

        public ICommand MenuItemCommand => new DelegateCommand<MenuItemVM>(
            x =>
            {
                regionManager.RequestNavigate(ViewRegions.MainContent, $"{x.EntityName}List",
                    new NavigationParameters
                    {
                        { "EntityInfo", repository.GetEntityByName(x.EntityName) }
                    });
            });

        public HomePageViewModel(ITDSRepository repository, IRegionManager regionManager)
        {
            this.repository = repository;
            this.regionManager = regionManager;
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

        #region INavigationAware
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }
        #endregion
    }
}
