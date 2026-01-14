using System;
using System.Data;
using System.Web.UI.WebControls;

namespace MiniProject_Karakara
{
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
                total += Convert.ToDecimal(row["TotalPrice"]);
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
        }

        protected void gvCart_Deleted(object sender, GridViewDeletedEventArgs e)
        {
            // This event is usually for SqlDataSource.With manual binding, we use RowDeleting 
            // and call BindCart() immediately, so this might not be fired or needed.
            // I'll leave it empty or handle UI message if forced.
            if (Session["CartDeletedItem"] != null)
            {
                lblMessage.Text = Session["CartDeletedItem"].ToString();
                lblMessage.ForeColor = System.Drawing.Color.Green;
                Session.Remove("CartDeletedItem");
            }
        }

        protected void btnConfirmOrder_Click(object sender, EventArgs e)
        {
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
        }
    }
}