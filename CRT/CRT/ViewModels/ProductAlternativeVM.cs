using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRT.ViewModels
{
    public class ProductAlternativeVM
    {
        public string ProductId { get; set; }
        public string ProductAlternativeId { get; set; }
        public DateTime Modified { get; set; }
        public string LanguageId { get; set; }
    }
}