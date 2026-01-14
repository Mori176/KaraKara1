using System;
using System.Collections.Generic;

using System.Web.UI.WebControls;

namespace MiniProject_Karakara
{
    // Inherit from BasePage for standardized logic (Design Pattern: Template Method)
    public partial class ProductPage : BasePage
    {
        // Use Repository (Design Pattern: Repository)
        ProductRepository _repo = new ProductRepository();
        CartRepository _cart = new CartRepository();

        public int SelectedProductID
        {
            get { return (int)(ViewState["SelectedProductID"] ?? 0); }
            set { ViewState["SelectedProductID"] = value; }
        }

        // Helper method to return css class
        public string GetProductCardCssClass(int productId)
        {
            return productId == SelectedProductID ? "product-card selected" : "product-card";
        }

        protected override bool RequiresAuthentication => true;

        protected override void OnLoadLogic(EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem("All Products", ""));
                
                // Load products via Repository instead of SqlDataSource if possible,
                // but since the ASPX has SqlDataSource, we might keep it simple or replace it.
                // For proper refactoring, we should bind ListView manually.
                BindProducts();
            }
        }

        private void BindProducts()
        {
            int catId = 0;
            if (!string.IsNullOrEmpty(ddlCategory.SelectedValue))
                int.TryParse(ddlCategory.SelectedValue, out catId);

            List<Product> products = _repo.GetProducts(catId);
            lvProducts.DataSource = products;
            lvProducts.DataBind();
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedProductID = 0;
            BindProducts();
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int productId = Convert.ToInt32(btn.CommandArgument);
            SelectedProductID = productId;
            BindProducts();
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";

            // Validate select
            if (SelectedProductID == 0)
            {
                lblMessage.Text = "Please select a product first.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // Validate quantity
            int quantity;
            if (!int.TryParse(txtQuantity.Text, out quantity) || quantity <= 0)
            {
                lblMessage.Text = "Please enter a valid quantity.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            try
            {
                int userId = GetUserId(); // Need a helper for this ideally, implementing simple fetch

                // Check Stock
                int currentStock = _repo.GetStock(SelectedProductID);
                if (currentStock < quantity)
                {
                    lblMessage.Text = $"Insufficient stock. Only {currentStock} allowed.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                // Add to Cart
                _cart.AddToCart(userId, SelectedProductID, quantity);

                // Get Product Name for success message
                Product p = _repo.GetProductById(SelectedProductID);

                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = "x" + quantity + " " + p.ProductName + " added to cart successfully!";
                
                SelectedProductID = 0;
                txtQuantity.Text = "";
                BindProducts();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private int GetUserId()
        {
            // Simple helper to get ID from Session Username.
            // Ideally User Object should be in Session.
            string username = Session["username"].ToString();
            return DatabaseHelper.Instance.ExecuteScalar<int>("SELECT Id FROM Users WHERE username = @U", cmd => cmd.Parameters.AddWithValue("@U", username));
        }
    }
}