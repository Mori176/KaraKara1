using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;

namespace MiniProject_Karakara
{
    public partial class LoginPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["account-created"] != null)
                {
                    lblMessage.Text = Session["account-created"].ToString();
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    Session.Remove("account-created"); 
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string hashedPassword = HashPassword(password);
            string dbConn = ConfigurationManager.ConnectionStrings["ConnKarakara"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(dbConn))
            {
                string query = "SELECT role,Id FROM Users WHERE username = @username AND password = @password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", hashedPassword);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // success
                        Session["username"] = username;
                        Session["role"] = reader["role"];
                        Session["userId"] = reader["Id"];

                        string role = reader["role"].ToString().ToLower();

                        if (role == "admin")
                        {
                            Response.Redirect("ManageUserPage.aspx");
                        }
                        else if (role == "customer")
                        {
                            Response.Redirect("ProductPage.aspx");
                        }
                    }
                    else
                    {
                        //failed
                        lblMessage.Text = "Invalid username or password.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;

                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                }
            }
        }

        // for password hash
        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}