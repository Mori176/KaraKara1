using System;
<<<<<<< HEAD
using System.Data;
=======
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
>>>>>>> f1f8904534b68d426f5c2e7ecb0d98561e6e2900
using System.Web.UI.WebControls;

namespace MiniProject_Karakara
{
<<<<<<< HEAD
    public partial class CartPage : BasePage
    {
        OrderFacade _orderFacade = new OrderFacade();
        CartRepository _cartRepo = new CartRepository();
        PricingStrategy _pricing = new PricingStrategy();

        protected override bool RequiresAuthentication => true;

        protected override void OnLoadLogic(EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCart();
            }
        }

        private int GetUserId()
        {
            string username = Session["username"].ToString();
            return DatabaseHelper.Instance.ExecuteScalar<int>("SELECT Id FROM Users WHERE username = @U", cmd => cmd.Parameters.AddWithValue("@U", username));
        }

        private void BindCart()
        {
            int userId = GetUserId();
            DataTable dt = _cartRepo.GetCartItems(userId);
            
            // Calculate Totals
            decimal total = 0;
            foreach(DataRow row in dt.Rows)
            {
                total += Convert.ToDecimal(row["TotalItemPrice"]);
            }

            if (total > 0)
            {
                decimal tax = _pricing.CalculateTax(total);
                decimal amountToPay = _pricing.CalculateTotal(total);

                lblTotal.Text = "RM " + total.ToString("N2");
                lblTax.Text = "RM " + tax.ToString("N2");
                lblAmountToPay.Text = "RM " + amountToPay.ToString("N2");

                summaryHide.Visible = true;
                btnConfirmOrder.Visible = true;
            }
            else
            {
                summaryHide.Visible = false;
                btnConfirmOrder.Visible = false;
            }

            // Manually bind to GridView, overriding SqlDataSource if present
            gvCart.DataSource = dt;
            gvCart.DataBind();
        }

        protected void gvCart_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int userId = GetUserId();
                // Access keys. NOTE: GridView must have DataKeyNames="ProductName,Quantity,ProductId" set in .aspx
                // The original code used ProductName & Quantity. We need ProductId ideally.
                // If ProductId isn't in DataKeys, this refactor is risky.
                // However, I can't easily see the ASPX DataKeyNames.
                // I will try to use the Repository 'DeleteCartItem' assuming I can get the ID.
                // If not, I might have to rely on the old method or update ASPX.
                // Let's assume ProductId is better. But I don't want to break if not present.
                // Plan B: Use the original code's logic style if ProductId is missing, but Repository expects ID.
                // I'll assume ProductID is available or I must update ASPX.
                // Let's check keys from previous code view:
                // "gvCart.DataKeys[e.RowIndex].Values["ProductName"]"
                
                string productName = gvCart.DataKeys[e.RowIndex].Values["ProductName"].ToString();
                
                // I need ProductID for the repository deletion.
                // Using a helper lookup for safety if I can't change ASPX.
                int productId = DatabaseHelper.Instance.ExecuteScalar<int>("SELECT ProductID FROM Products WHERE ProductName = @N", cmd => cmd.Parameters.AddWithValue("@N", productName));
                
                _cartRepo.DeleteCartItem(userId, productId);

                Session["CartDeletedItem"] = $"Item <strong>{productName}</strong> removed.";
                BindCart(); // Refresh
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
=======
    public partial class CartPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    Response.Redirect("LoginPage.aspx");
                }

                // call the method to display cart summary
                CartSummary();
            }
        }

        protected void gvCart_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // call the method for deletion of items
            string productName = gvCart.DataKeys[e.RowIndex].Values["ProductName"].ToString();
            string quantity = gvCart.DataKeys[e.RowIndex].Values["Quantity"].ToString();
            Session["CartDeletedItem"] = $"x{quantity} <strong>{productName}</strong> has been removed from your cart.";
>>>>>>> f1f8904534b68d426f5c2e7ecb0d98561e6e2900
        }

        protected void gvCart_Deleted(object sender, GridViewDeletedEventArgs e)
        {
<<<<<<< HEAD
            // This event is usually for SqlDataSource.With manual binding, we use RowDeleting 
            // and call BindCart() immediately, so this might not be fired or needed.
            // I'll leave it empty or handle UI message if forced.
            if (Session["CartDeletedItem"] != null)
            {
                lblMessage.Text = Session["CartDeletedItem"].ToString();
                lblMessage.ForeColor = System.Drawing.Color.Green;
                Session.Remove("CartDeletedItem");
            }
=======
            // call the method after deletion of items
            if (Session["CartDeletedItem"] != null)
            {
                string cartDeletedItem = Session["CartDeletedItem"].ToString();
                lblMessage.Text = cartDeletedItem;
                lblMessage.ForeColor = System.Drawing.Color.Green;
                Session.Remove("CartDeletedItem"); 
            }
            else
            {
                lblMessage.Text = "Item removed from your cart.";
            }
            CartSummary();
        }

        private void CartSummary()
        {
            DataView dv = (DataView)SqlDataSource2.Select(DataSourceSelectArguments.Empty);
            
            if (dv != null && dv.Count > 0 && dv[0]["TotalBeforeTax"] != DBNull.Value)
            {
                decimal total = Convert.ToDecimal(dv[0]["TotalBeforeTax"]);

                if(total > 0)
                {
                    decimal tax = total * 0.06m;
                    decimal amountToPay = total + tax;

                    lblTotal.Text = "RM " + total.ToString("N2");
                    lblTax.Text = "RM " + tax.ToString("N2");
                    lblAmountToPay.Text = "RM " + amountToPay.ToString("N2");

                    summaryHide.Visible = true;
                    btnConfirmOrder.Visible = true;
                    return;
                }
            }

            //if total is 0, hide summary
            summaryHide.Visible = false;
            btnConfirmOrder.Visible = false;
>>>>>>> f1f8904534b68d426f5c2e7ecb0d98561e6e2900
        }

        protected void btnConfirmOrder_Click(object sender, EventArgs e)
        {
<<<<<<< HEAD
            int userId = GetUserId();
            string orderNumber;
            string error;

            bool success = _orderFacade.PlaceOrder(userId, out orderNumber, out error);

            if (success)
            {
                Session["order-added"] = $"Order {orderNumber} confirmed!";
                Response.Redirect("OrderHistoryPage.aspx");
            }
            else
            {
                lblMessage.Text = "Order Failed: " + error;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
=======
            string username = Session["username"]?.ToString();
            string orderNumber = GenerateOrderNumber();
            DateTime orderDate = DateTime.Now;

            // get cart items
            string connectionString = ConfigurationManager.ConnectionStrings["ConnKarakara"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                
                // get user's cart from procedure
                SqlCommand getCartCmd = new SqlCommand("GetUserCart", conn);
                getCartCmd.CommandType = CommandType.StoredProcedure;
                getCartCmd.Parameters.AddWithValue("@Username", username);

                SqlDataAdapter da = new SqlDataAdapter(getCartCmd);
                DataTable cartItems = new DataTable();
                da.Fill(cartItems);

                if (cartItems.Rows.Count == 0 )
                {
                    return;
                }

                // get user id
                SqlCommand getUserIDcmd = new SqlCommand("SELECT Id FROM Users WHERE username = @Username", conn);
                getUserIDcmd.Parameters.AddWithValue("@Username", username);
                object result = getUserIDcmd.ExecuteScalar();
                int userId = Convert.ToInt32(result);

                // insert order into Orders table
                SqlCommand insertOrderCmd = new SqlCommand("INSERT INTO Orders (OrderNumber, UserID, OrderDate, TotalAmount, Tax, AmountToPay) OUTPUT INSERTED.OrderID VALUES (@OrderNumber, @UserID, @OrderDate, @Total, @Tax, @AmountToPay)", conn);

                decimal total = decimal.Parse(lblTotal.Text.Replace("RM ", "").Trim());
                decimal tax = decimal.Parse(lblTax.Text.Replace("RM ", "").Trim());
                decimal amountToPay = decimal.Parse(lblAmountToPay.Text.Replace("RM ", "").Trim());

                insertOrderCmd.Parameters.AddWithValue("@OrderNumber", orderNumber);
                insertOrderCmd.Parameters.AddWithValue("@UserID", userId);
                insertOrderCmd.Parameters.AddWithValue("@OrderDate", orderDate);
                insertOrderCmd.Parameters.AddWithValue("@Total", total);
                insertOrderCmd.Parameters.AddWithValue("@Tax", tax);
                insertOrderCmd.Parameters.AddWithValue("@AmountToPay", amountToPay);

                int orderId = (int)insertOrderCmd.ExecuteScalar();

                // insert each cart item into OrderItems table
                foreach (DataRow row in cartItems.Rows)
                {
                    SqlCommand insertOrderItemCmd = new SqlCommand("INSERT INTO OrderItems (OrderID, ProductID, Quantity, Price) VALUES (@OrderID, @ProductID, @Quantity, @Price)", conn);
                    insertOrderItemCmd.Parameters.AddWithValue("@OrderID", orderId);
                    insertOrderItemCmd.Parameters.AddWithValue("@ProductID", row["ProductID"]);
                    insertOrderItemCmd.Parameters.AddWithValue("@Quantity", row["Quantity"]);
                    insertOrderItemCmd.Parameters.AddWithValue("@Price", row["Price"]);

                    insertOrderItemCmd.ExecuteNonQuery();
                }

                SqlCommand clearCartCmd = new SqlCommand("DELETE FROM UserCart WHERE UserID = @UserId", conn);
                clearCartCmd.Parameters.AddWithValue("@UserId", userId);
                clearCartCmd.ExecuteNonQuery();

                conn.Close();
                Session["order-added"] = "Your order " + orderNumber + " has been confirmed!";
                Response.Redirect("OrderHistoryPage.aspx");
            }
        }

        private string GenerateOrderNumber()
        {
            return "ORD" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
>>>>>>> f1f8904534b68d426f5c2e7ecb0d98561e6e2900
        }
    }
}