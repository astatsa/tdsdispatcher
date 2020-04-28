using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using TDSDispatcher.Models;

namespace TDSDispatcher.Services
{
    public interface ISelectable
    {
        object Select(string refName, object selectedItem = null, Window owner = null, object filterParameter = null);
    }
}
