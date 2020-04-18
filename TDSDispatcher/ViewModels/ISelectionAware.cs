using System;
using System.Collections.Generic;
using System.Text;

namespace TDSDispatcher.ViewModels
{
    interface ISelectionAware
    {
        event EventHandler<object> Selected;
    }
}
