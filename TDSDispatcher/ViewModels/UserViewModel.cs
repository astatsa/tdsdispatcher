using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using TDSDispatcher.Extensions;
using TDSDispatcher.Repositories;
using TDSDispatcher.Services;
using TDSDTO.References;

namespace TDSDispatcher.ViewModels
{
    class UserViewModel : BaseEntityViewModel<User>
    {
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }

        private Role role;
        public Role Role
        {
            get => role;
            set => SetProperty(ref role, value);
        }


        public UserViewModel(ReferenceService referenceService, ITdsApiService apiService, IDialogService dialogService, ITDSRepository repository) : base(referenceService, apiService, dialogService, repository)
        {
        }

        protected override void OnBeforeSave(ref bool cancel)
        {
            if(Password != PasswordConfirm)
            {
                dialogService.ShowMessageBox("Ошибка", "Пароль и его подтверждение не совпадают!");
                cancel = true;
                return;
            }

            if(Password.Length > 0)
                Model.PasswordHash = LoginViewModel.GetPasswordHash(Password);
        }

        protected override async void ModelChanged()
        {
            Role = await GetEntityByIdAsync<Role>(Model.RoleId);
        }
    }
}
