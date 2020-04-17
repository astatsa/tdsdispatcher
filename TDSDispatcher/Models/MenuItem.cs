using System;
using System.Collections.Generic;
using System.Text;

namespace TDSDispatcher.Models
{
    class MenuItem
    {
        public string Title { get; set; }
        public string ModelName { get; set; }
        public string URL { get; set; }
        public List<MenuItem> Childs { get; set; }
    }
}
