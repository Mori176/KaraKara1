using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiniProject_Karakara
{
    public partial class OrderHistoryPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Session["username"] == null || Session["userId"] == null)
                {
                    Response.Redirect("LoginPage.aspx");
                }

                if (Session["order-added"] != null)
                {
                    lblMessage.Text = Session["order-added"].ToString();
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    Session.Remove("order-added");
                }
                LoadOrderHistory();
            }
        }

        private void LoadOrderHistory()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnKarakara"].ConnectionString);
            SqlCommand cmd = new SqlCommand("GetUserOrderHistory", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", Session["userId"]);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                // if no order found
                emptyOrderHistory.Visible = true;
                Repeater1.Visible = false;
                return;
            }

            // group by OrderNumber
            var grouped = dt.AsEnumerable()
                .GroupBy(r => r.Field<string>("OrderNumber"))
                .Select(g => new {
                    OrderNumber = g.Key,
                    // Assuming OrderID is same for the group, take first
                    OrderID = g.First().Field<int>("OrderID"), 
                    OrderDate = g.First().Field<DateTime>("OrderDate"),
                    TotalAmount = g.First().Field<decimal>("TotalAmount"),
                    Tax = g.First().Field<decimal>("Tax"),
                    AmountToPay = g.First().Field<decimal>("AmountToPay"),
                    // Check if Status column exists (if script run), else default
                    Status = g.First().Table.Columns.Contains("Status") ? g.First().Field<string>("Status") : "Pending",
                    Items = g.CopyToDataTable()
                });

            Repeater1.Visible = true;
            emptyOrderHistory.Visible = false;
            Repeater1.DataSource = grouped;
            Repeater1.DataBind();
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                dynamic data = e.Item.DataItem;
                DataTable itemsTable = data.Items;

                Repeater innerRepeater = (Repeater)e.Item.FindControl("Repeater2");
                innerRepeater.DataSource = itemsTable;
                innerRepeater.DataBind();
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                string[] args = btn.CommandArgument.ToString().Split('|');
                string orderNumber = args[0];
                string currentStatus = args[1];

                int orderId = DatabaseHelper.Instance.ExecuteScalar<int>("SELECT OrderID FROM Orders WHERE OrderNumber = @N", cmd => cmd.Parameters.AddWithValue("@N", orderNumber));

                // Design Pattern 7: State Pattern Usage
                OrderContext context = new OrderContext(orderId, currentStatus);
                context.Cancel();

                lblMessage.Text = $"Order {orderNumber} has been cancelled.";
                lblMessage.ForeColor = System.Drawing.Color.Green;

                LoadOrderHistory(); // Refresh UI
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error cancelling order: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

    }
}