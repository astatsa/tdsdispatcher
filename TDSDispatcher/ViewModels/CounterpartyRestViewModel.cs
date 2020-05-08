using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TDSDispatcher.Extensions;
using TDSDispatcher.Repositories;
using TDSDispatcher.Services;
using TDSDTO.Documents;

namespace TDSDispatcher.ViewModels
{
    class CounterpartyRestViewModel : BindableBase, IDialogAware
    {
        private readonly IDialogService dialogService;
        private readonly ITDSRepository repository;
        #region Commands
        public ICommand SaveCloseCommand => new DelegateCommand(
            async () =>
            {
                if(await Save())
                    RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            });
        #endregion

        #region Properties
        private CounterpartyRestCorrection document;
        public CounterpartyRestCorrection Document
        {
            get => document;
            set => SetProperty(ref document, value);
        }

        #endregion

        #region IDialogAware implementation
        public string Title => "Остатки поставщиков";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
            
        }

        public async void OnDialogOpened(IDialogParameters parameters)
        {
            int counterpartyId = 0;
            if (parameters == null || !parameters.TryGetValue("CounterpartyId", out counterpartyId))
            {
                dialogService.ShowMessageBox("Ошибка", "Не указан идентификатор элемента справочника!", new ButtonResult[] { ButtonResult.OK });
                RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
            }

            var rests = await repository.GetRestsByCounterpartyId(counterpartyId);
            this.Document = new CounterpartyRestCorrection
            {
                CounterpartyId = counterpartyId,
                Date = DateTime.Now,
                MaterialCorrections = rests.Select(x => new CounterpartyRestCorrectionMaterial
                {
                    MaterialId = x.MaterialId,
                    MaterialName = x.MaterialName,
                    Correction = x.Rest
                }).ToList()
            };
        }
        #endregion

        public CounterpartyRestViewModel(IDialogService dialogService, ITDSRepository repository)
        {
            this.dialogService = dialogService;
            this.repository = repository;
        }

        private async Task<bool> Save()
        {
            try
            {
                return await repository.SaveReferenceAsync(document);
            }
            catch (Exception ex)
            {
                dialogService.ShowMessageBox("Ошибка", ex.Message, new ButtonResult[] { ButtonResult.OK });
                return false;
            }
        }
    }
}
