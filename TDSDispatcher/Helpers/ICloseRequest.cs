using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace TDSDispatcher.Helpers
{
    interface ICloseRequest
    {
        event EventHandler<bool> CloseRequest;
    }
}
