using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TDSDispatcher.Services;
using TDSDTO.References;

namespace TDSDispatcher.ViewModels
{
    class EmployeeViewModel : BaseEntityViewModel<Employee>
    {
        public EmployeeViewModel(ReferenceService referenceService, ITdsApiService apiService) : base(referenceService, apiService)
        {
        }
    }
}
