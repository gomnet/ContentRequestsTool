﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRT.ViewModels
{
    public class ProductVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Modified { get; set; }
        public string RiskId { get; set; }
        public string Description { get; set; }
        public string LanguageId { get; set; }
    }
}