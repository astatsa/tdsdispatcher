using System;
using System.Collections.Generic;
using System.Text;
using TDSDispatcher.Models;
using TDSDTO.References;

namespace TDSDispatcher
{
    class SessionContext
    {
        public string Token { get; set; }
        public Employee Employee { get; set; }
        public ICollection<string> Permissions { get; set; }
        public string UserFullName { get; set; }
    }
}
