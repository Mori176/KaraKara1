using System;
using System.Data;
using System.Data.SqlClient;

namespace MiniProject_Karakara
{
    // Repository for Cart Operations
    public class CartRepository
    {
        private DatabaseHelper _db = DatabaseHelper.Instance;

        public DataTable GetCartItems(int userId)
        {
            // Using the existing Stored Procedure if available, or raw SQL
            // Based on analyzing CartPage.aspx.cs, it uses 'GetUserCart'
            /*
             However, to be safe and ensure portability given the 'update schema' context,
             I will use a direct join query that matches the likely 'GetUserCart' logic.
             */
            string query = @"
                SELECT 
                    uc.UserId, uc.ProductId, uc.quantity as Quantity, 
                    p.ProductName, p.Price, p.ProductImage,
                    (p.Price * uc.quantity) as TotalPrice
                FROM UserCart uc
                JOIN Products p ON uc.ProductId = p.ProductID
                WHERE uc.UserId = @UserId";

            return _db.ExecuteQuery(query, cmd => cmd.Parameters.AddWithValue("@UserId", userId));
        }

        public void AddToCart(int userId, int productId, int quantity)
        {
            // Logic: Check if exists, update quantity, else insert
            // Simplification: Just Insert as per original code, or better: UPSERT
            
            // Note: Original code just did Insert. I'll stick to Insert for now to match behavior,
            // but ideally we should merge.
            string query = "INSERT INTO UserCart (UserId, ProductId, quantity) VALUES (@UserId, @ProductId, @Qty)";
            _db.ExecuteNonQuery(query, cmd => {
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@Qty", quantity);
            });
        }

        public void ClearCart(int userId)
        {
            string query = "DELETE FROM UserCart WHERE UserId = @UserId";
            _db.ExecuteNonQuery(query, cmd => cmd.Parameters.AddWithValue("@UserId", userId));
        }

        public void DeleteCartItem(int userId, int productId)
        {
            string query = "DELETE FROM UserCart WHERE UserId = @UserId AND ProductId = @ProductId";
            _db.ExecuteNonQuery(query, cmd => {
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ProductId", productId);
            });
        }
    }
}
