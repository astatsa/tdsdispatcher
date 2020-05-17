using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TDSDispatcher.Models;

namespace TDSDispatcher.ViewModels.Dialogs
{
    class QuickSearchViewModel : BindableBase, IDialogAware
    {
        private string searchText;
        public string SearchText 
        { 
            get => searchText;
            set => SetProperty(ref searchText, value); 
        }

        private ICollection<EntityColumn> searchPlaces;
        public ICollection<EntityColumn> SearchPlaces
        {
            get => searchPlaces;
            set => SetProperty(ref searchPlaces, value);
        }

        private EntityColumn currentSearchPlace;
        public EntityColumn CurrentSearchPlace
        {
            get => currentSearchPlace;
            set => SetProperty(ref currentSearchPlace, value);
        }


        private ICommand closeCommand;
        public ICommand CloseCommand =>
            closeCommand ?? (closeCommand = new DelegateCommand(
                () =>
                {
                    RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
                }));

        private ICommand searchCommand;
        public ICommand SearchCommand =>
            searchCommand ?? (searchCommand = new DelegateCommand(
                () =>
                {
                    RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters
                    {
                        { "SearchText", SearchText },
                        { "SearchPlace", CurrentSearchPlace }
                    }));
                }));


        #region IDialogAware
        public string Title => "Поиск";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if(parameters.TryGetValue("SearchText", out string text))
            {
                SearchText = text;
            }
            if(parameters.TryGetValue("SearchPlaces", out ICollection<EntityColumn> places))
            {
                SearchPlaces = places;
            }
            if(parameters.TryGetValue("CurrentSearchPlace", out EntityColumn currentPlace))
            {
                CurrentSearchPlace = currentPlace;
            }
        }
        #endregion
    }
}
