using System;

namespace MiniProject_Karakara
{
    // Domain Model
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string ProductImage { get; set; }
        public string ProductDescription { get; set; }
        public int CategoryID { get; set; }
        public int StockQuantity { get; set; } // New feature

        public bool IsInStock => StockQuantity > 0;
    }
}
