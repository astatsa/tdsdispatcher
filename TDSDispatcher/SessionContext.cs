using System;
using System.Collections.Generic;
using System.Text;
using TDSDispatcher.Models;

namespace TDSDispatcher
{
    class SessionContext
    {
        public string Token { get; set; }
        public Employee Employee { get; set; }
    }
}
