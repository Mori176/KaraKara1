using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiniProject_Karakara
{
    public partial class ManageUserPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null || Session["role"].ToString() != "admin")
                {
                    Session.RemoveAll();
                    Response.Redirect("LoginPage.aspx");
                }
            }
        }

        protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvUsers.Rows[e.RowIndex];
            string userId = gvUsers.DataKeys[e.RowIndex].Value.ToString();

            string username = ((TextBox)row.Cells[1].Controls[0]).Text;
            string role = ((TextBox)row.Cells[2].Controls[0]).Text;
            string email = ((TextBox)row.Cells[3].Controls[0]).Text;

            TextBox txtPassword = (TextBox)row.FindControl("txtPassword");
            string password = txtPassword.Text;

            string hashedPassword = string.IsNullOrWhiteSpace(password)
                ? GetExistingPasswordFromDb(userId) // fetch current password if empty
                : HashPassword(password); // hash new password

            SqlDataSource1.UpdateParameters["username"].DefaultValue = username;
            SqlDataSource1.UpdateParameters["role"].DefaultValue = role;
            SqlDataSource1.UpdateParameters["email"].DefaultValue = email;
            SqlDataSource1.UpdateParameters["password"].DefaultValue = hashedPassword;
            SqlDataSource1.UpdateParameters["Id"].DefaultValue = userId;
        }

        public string HashPassword(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));  // convert to hex string
                }
                return builder.ToString();
            }
        }

        private string GetExistingPasswordFromDb(string userId)
        {
            string password = "";
            string connStr = ConfigurationManager.ConnectionStrings["ConnKarakara"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT password FROM Users WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", userId);
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                    password = result.ToString();
            }
            return password;
        }
    }
}