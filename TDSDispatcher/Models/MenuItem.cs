﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TDSDispatcher.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string EntityName { get; set; }
        public int ParentId { get; set; }
    }
}
