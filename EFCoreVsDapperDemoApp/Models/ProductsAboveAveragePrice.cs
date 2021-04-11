using System;
using System.Collections.Generic;

#nullable disable

namespace EFCoreVsDapperDemoApp.Models
{
    public partial class ProductsAboveAveragePrice
    {
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
