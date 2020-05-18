using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TDSDispatcher.Repositories;
using TDSDispatcher.Services;
using TDSDTO.Documents;
using TDSDTO.Filter;
using TDSDTO.References;

namespace TDSDispatcher.ViewModels
{
    class OrderViewModel : BaseEntityViewModel<Order>
    {
        #region Properties
        public Filter SupplierFilter =>
            new FilterConditionGroup
            {
                Conditions = new FilterConditionCollection
                {
                    new FilterCondition<FieldOperand, bool>(
                        new FieldOperand("IsSupplier"),
                        true,
                        ConditionOperation.Equal)
                }
            };

        public Filter CustomerFilter =>
            new FilterConditionGroup
            {
                Conditions = new FilterConditionCollection
                {
                    new FilterCondition<FieldOperand, bool>(
                        new FieldOperand("IsSupplier"),
                        false,
                        ConditionOperation.Equal)
                }
            };

        private Counterparty supplier;
        public Counterparty Supplier
        {
            get => supplier;
            set => SetProperty(ref supplier, value, () => Model.SupplierId = Supplier?.Id ?? 0);
        }

        private Counterparty customer;
        public Counterparty Customer
        {
            get => customer;
            set => SetProperty(ref customer, value, () => Model.CustomerId = Customer?.Id ?? 0);
        }

        private Material material;
        public Material Material
        {
            get => material;
            set => SetProperty(ref material, value, () => Model.MaterialId = Material?.Id ?? 0);
        }

        private Employee driver;
        public Employee Driver
        {
            get => driver;
            set => SetProperty(ref driver, value, () => Model.DriverId = Driver?.Id ?? 0);
        }


        #endregion

        public OrderViewModel(ReferenceService referenceService, ITdsApiService apiService, IDialogService dialogService, ITDSRepository repository) 
            : base(referenceService, apiService, dialogService, repository)
        {
        }

        protected override async void ModelChanged()
        {
            var supp = GetEntityByIdAsync<Counterparty>(Model.SupplierId);
            var cust = GetEntityByIdAsync<Counterparty>(Model.CustomerId);
            var mat = GetEntityByIdAsync<Material>(Model.MaterialId);
            var driv = GetEntityByIdAsync<Employee>(Model.DriverId);

            if (Model.Date == null)
                Model.Date = DateTime.Now;

            Supplier = await supp;
            Customer = await cust;
            Material = await mat;
            Driver = await driv;
            base.ModelChanged();
        }
    }
}
