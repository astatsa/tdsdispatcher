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

namespace TDSDispatcher.ViewModels
{
    class EmployeeViewModel : BaseEntityViewModel
    {
        public EmployeeViewModel(ReferenceService referenceService) : base(referenceService)
        {
        }
    }
}
