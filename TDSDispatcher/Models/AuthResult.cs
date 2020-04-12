using System;
using System.Collections.Generic;
using System.Text;
using TDSDTO.References;

namespace TDSDispatcher.Models
{
    class AuthResult
    {
        public string Token { get; set; }
        public Employee Employee { get; set; }
    }
}
