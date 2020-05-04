using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TDSDispatcher.Repositories;
using TDSDispatcher.Services;
using TDSDTO.References;

namespace TDSDispatcher.ViewModels
{
    class EmployeeViewModel : BaseEntityViewModel<Employee>
    {
        private Position position;
        public Position Position
        {
            get => position;
            set => SetProperty(ref position, value, () => Model.PositionId = position.Id);
        }

        private User user;
        public User User
        {
            get => user;
            set => SetProperty(ref user, value, () => Model.UserId = user.Id);
        }


        public EmployeeViewModel(ReferenceService referenceService, ITdsApiService apiService, IDialogService dialogService, ITDSRepository repository) 
            : base(referenceService, apiService, dialogService, repository)
        {
        }

        protected async override void ModelChanged()
        {
            await Task.WhenAll(
                SetEntityByIdAsync<Position>(Model.PositionId, x => Position = x),
                SetEntityByIdAsync<User>(Model.UserId, x => User = x)
                );

            base.ModelChanged();
        }
    }
}
