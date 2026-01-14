using System;
using System.Data;

namespace MiniProject_Karakara
{
    // Design Pattern 2: Factory Method (Static Factory)
    // Centralizes the creation logic of domain objects from DataRows, 
    // isolating the complexity of mapping database columns to object properties.
    public static class ModelFactory
    {
        public static Product CreateProduct(DataRow row)
        {
            if (row == null) return null;

            return new Product
            {
                ProductID = Convert.ToInt32(row["ProductID"]),
                ProductName = row["ProductName"].ToString(),
                Price = Convert.ToDecimal(row["Price"]),
                ProductImage = row["ProductImage"].ToString(),
                ProductDescription = row["ProductDescription"].ToString(),
                CategoryID = Convert.ToInt32(row["CategoryID"]),
                // Handle possibly missing column for legacy DBs without migration
                StockQuantity = row.Table.Columns.Contains("StockQuantity") 
                    ? (row["StockQuantity"] == DBNull.Value ? 0 : Convert.ToInt32(row["StockQuantity"])) 
                    : 10 // Default fallback if script not run yet
            };
        }
    }
}
