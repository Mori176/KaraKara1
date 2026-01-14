using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MiniProject_Karakara
{
    // Design Pattern 3: Repository
    // Abstract the data layer, allowing the rest of the app to work with 'Product' objects
    // instead of SQL queries.
    public class ProductRepository
    {
        private DatabaseHelper _db = DatabaseHelper.Instance;

        public List<Product> GetProducts(int categoryId = 0)
        {
            List<Product> list = new List<Product>();
            string query = "SELECT * FROM Products WHERE (@CatID = 0 OR CategoryID = @CatID)";

            DataTable dt = _db.ExecuteQuery(query, cmd => cmd.Parameters.AddWithValue("@CatID", categoryId));

            foreach (DataRow row in dt.Rows)
            {
                list.Add(ModelFactory.CreateProduct(row));
            }

            return list;
        }

        public Product GetProductById(int productId)
        {
            string query = "SELECT * FROM Products WHERE ProductID = @ID";
            DataTable dt = _db.ExecuteQuery(query, cmd => cmd.Parameters.AddWithValue("@ID", productId));

            if (dt.Rows.Count > 0)
                return ModelFactory.CreateProduct(dt.Rows[0]);
            
            return null;
        }

        public void DeductStock(int productId, int quantity)
        {
            string query = "UPDATE Products SET StockQuantity = StockQuantity - @Qty WHERE ProductID = @ID";
            _db.ExecuteNonQuery(query, cmd => {
                cmd.Parameters.AddWithValue("@Qty", quantity);
                cmd.Parameters.AddWithValue("@ID", productId);
            });
        }
        
        public int GetStock(int productId)
        {
             string query = "SELECT StockQuantity FROM Products WHERE ProductID = @ID";
             return _db.ExecuteScalar<int>(query, cmd => cmd.Parameters.AddWithValue("@ID", productId));
        }
    }
}
