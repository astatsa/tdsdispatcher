using Prism.Services.Dialogs;
using TDSDispatcher.Repositories;
using TDSDispatcher.Services;
using TDSDTO.Documents;
using TDSDTO.References;

namespace TDSDispatcher.ViewModels
{
    class RefuelViewModel : BaseEntityViewModel<Refuel>
    {
        private Employee driver;
        public Employee Driver
        {
            get => driver;
            set => SetProperty(ref driver, value, () => Model.DriverId = driver.Id);
        }

        private GasStation gasStation;
        public GasStation GasStation
        {
            get => gasStation;
            set => SetProperty(ref gasStation, value, () => Model.GasStationId = gasStation.Id);
        }



        public RefuelViewModel(ReferenceService referenceService, ITdsApiService apiService, IDialogService dialogService, ITDSRepository repository) 
            : base(referenceService, apiService, dialogService, repository)
        {
        }

        protected override async void ModelChanged()
        {
            var driver = GetEntityByIdAsync<Employee>(Model.DriverId);
            var station = GetEntityByIdAsync<GasStation>(Model.GasStationId);

            Driver = await driver;
            GasStation = await station;
            base.ModelChanged();
        }
    }
}
