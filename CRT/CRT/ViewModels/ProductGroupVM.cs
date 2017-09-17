using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRT.ViewModels
{
    public class ProductGroupVM
    {
        public string GroupId { get; set; }
        public string ProductId { get; set; }
        public DateTime Modified { get; set; }
        public string LanguageId { get; set; }
    }
}