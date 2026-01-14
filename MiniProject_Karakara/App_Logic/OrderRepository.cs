using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MiniProject_Karakara
{
    public class OrderRepository
    {
        private DatabaseHelper _db = DatabaseHelper.Instance;

        public int CreateOrder(int userId, string orderNumber, DateTime date, decimal total, decimal tax, decimal grandTotal, DataTable cartItems)
        {
            int orderId = 0;

            using (SqlConnection conn = _db.GetConnection())
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Insert Order
                        string orderQuery = @"INSERT INTO Orders 
                            (OrderNumber, UserID, OrderDate, TotalAmount, Tax, AmountToPay) 
                            VALUES (@OrderNumber, @UserID, @OrderDate, @Total, @Tax, @AmountToPay);
                            SELECT SCOPE_IDENTITY();";

                        using (SqlCommand cmd = new SqlCommand(orderQuery, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@OrderNumber", orderNumber);
                            cmd.Parameters.AddWithValue("@UserID", userId);
                            cmd.Parameters.AddWithValue("@OrderDate", date);
                            cmd.Parameters.AddWithValue("@Total", total);
                            cmd.Parameters.AddWithValue("@Tax", tax);
                            cmd.Parameters.AddWithValue("@AmountToPay", grandTotal);

                            object result = cmd.ExecuteScalar();
                            orderId = Convert.ToInt32(result);
                        }

                        // 2. Insert Order Items
                        foreach (DataRow row in cartItems.Rows)
                        {
                            string itemQuery = @"INSERT INTO OrderItems 
                                (OrderID, ProductID, Quantity, Price) 
                                VALUES (@OrderID, @ProductID, @Quantity, @Price)";

                            using (SqlCommand cmd = new SqlCommand(itemQuery, conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@OrderID", orderId);
                                cmd.Parameters.AddWithValue("@ProductID", row["ProductId"]);
                                cmd.Parameters.AddWithValue("@Quantity", row["Quantity"]);
                                cmd.Parameters.AddWithValue("@Price", row["Price"]);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
            return orderId;
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            string query = "UPDATE Orders SET Status = @Status WHERE OrderID = @ID";
            _db.ExecuteNonQuery(query, cmd => {
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@ID", orderId);
            });
        }
    }
}
