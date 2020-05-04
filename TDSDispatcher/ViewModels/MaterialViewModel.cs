using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using TDSDispatcher.Repositories;
using TDSDispatcher.Services;
using TDSDTO.References;

namespace TDSDispatcher.ViewModels
{
    class MaterialViewModel : BaseEntityViewModel<Material>
    {
        private Measure measure;

        public Measure Measure
        {
            get => measure;
            set => SetProperty(ref measure, value, () => Model.MeasureId = measure.Id);
        }


        public MaterialViewModel(ReferenceService referenceService, ITdsApiService apiService, IDialogService dialogService, ITDSRepository repository)
            : base(referenceService, apiService, dialogService, repository)
        {

        }

        protected async override void ModelChanged()
        {
            base.ModelChanged();

            await SetEntityByIdAsync<Measure>(Model.MeasureId, x => Measure = x);
        }
    }
}
