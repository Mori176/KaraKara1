using System;
using System.Collections.Generic;
<<<<<<< HEAD
=======
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
>>>>>>> f1f8904534b68d426f5c2e7ecb0d98561e6e2900
using System.Web.UI.WebControls;

namespace MiniProject_Karakara
{
<<<<<<< HEAD
    // Inherit from BasePage for standardized logic (Design Pattern: Template Method)
    public partial class ProductPage : BasePage
    {
        // Use Repository (Design Pattern: Repository)
        ProductRepository _repo = new ProductRepository();
        CartRepository _cart = new CartRepository();

=======
    public partial class ProductPage : System.Web.UI.Page
    {
        // property to track selected product
>>>>>>> f1f8904534b68d426f5c2e7ecb0d98561e6e2900
        public int SelectedProductID
        {
            get { return (int)(ViewState["SelectedProductID"] ?? 0); }
            set { ViewState["SelectedProductID"] = value; }
        }

<<<<<<< HEAD
        // Helper method to return css class
=======
        // helper method to return css class
>>>>>>> f1f8904534b68d426f5c2e7ecb0d98561e6e2900
        public string GetProductCardCssClass(int productId)
        {
            return productId == SelectedProductID ? "product-card selected" : "product-card";
        }

<<<<<<< HEAD
        protected override bool RequiresAuthentication => true;

        protected override void OnLoadLogic(EventArgs e)
=======
        protected void Page_Load(object sender, EventArgs e)
>>>>>>> f1f8904534b68d426f5c2e7ecb0d98561e6e2900
        {
            if (!IsPostBack)
            {
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem("All Products", ""));
<<<<<<< HEAD
                
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
=======

                if (Session["username"] == null)
                {
                    Response.Redirect("LoginPage.aspx");
                }
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedProductID = 0;
>>>>>>> f1f8904534b68d426f5c2e7ecb0d98561e6e2900
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int productId = Convert.ToInt32(btn.CommandArgument);
            SelectedProductID = productId;
<<<<<<< HEAD
            BindProducts();
=======
            lvProducts.DataBind();

>>>>>>> f1f8904534b68d426f5c2e7ecb0d98561e6e2900
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";

<<<<<<< HEAD
            // Validate select
=======
            // validate selected product
>>>>>>> f1f8904534b68d426f5c2e7ecb0d98561e6e2900
            if (SelectedProductID == 0)
            {
                lblMessage.Text = "Please select a product first.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

<<<<<<< HEAD
            // Validate quantity
=======
            // validate quantity input
>>>>>>> f1f8904534b68d426f5c2e7ecb0d98561e6e2900
            int quantity;
            if (!int.TryParse(txtQuantity.Text, out quantity) || quantity <= 0)
            {
                lblMessage.Text = "Please enter a valid quantity.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

<<<<<<< HEAD
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
=======
            string username = Session["username"].ToString();
            int productId = SelectedProductID;
            string connDB = ConfigurationManager.ConnectionStrings["ConnKarakara"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connDB))
            {
                conn.Open();

                // get user id from username
                int userId = 0;
                using (SqlCommand cmd = new SqlCommand("SELECT Id FROM Users WHERE username = @username", conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    object result = cmd.ExecuteScalar();
                    if (result == null)
                    {
                        lblMessage.Text = "User not found.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    userId = Convert.ToInt32(result);
                }

                // get product name
                string productName = string.Empty;
                using (SqlCommand cmd = new SqlCommand("SELECT ProductName FROM Products WHERE ProductID = @productId", conn))
                {
                    cmd.Parameters.AddWithValue("@productId", productId);
                    object result = cmd.ExecuteScalar();
                    if (result == null)
                    {
                        lblMessage.Text = "Product not found.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    productName = result.ToString();
                }

                // insert to user cart
                using (SqlCommand cmd = new SqlCommand("INSERT INTO UserCart (UserId, ProductId, quantity) VALUES (@userId, @productId, @quantity)", conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@productId", productId);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.ExecuteNonQuery();
                }

                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = "x" + quantity + " " + productName + " added to cart successfully!";
                SelectedProductID = 0;
                txtQuantity.Text = "";
                lvProducts.DataBind();
            }
>>>>>>> f1f8904534b68d426f5c2e7ecb0d98561e6e2900
        }
    }
}