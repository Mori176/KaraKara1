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
    public partial class RegisterPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            // admin hash value in db = 240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9 = admin123
            string username = txtUsername.Text.Trim();
            string role = "customer";
            string email = txtEmail.Text.Trim();
            string password = HashPassword(txtPassword.Text.Trim());
            string dbConn = ConfigurationManager.ConnectionStrings["ConnKarakara"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(dbConn))
            {
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE username = @username OR email = @email";
                string insertQuery = "INSERT INTO Users (username, role, email, password) VALUES (@username, @role, @email, @password)";

                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@username", username);
                checkCmd.Parameters.AddWithValue("@email", email);

                SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@username", username);
                insertCmd.Parameters.AddWithValue("@role", role);
                insertCmd.Parameters.AddWithValue("@email", email);
                insertCmd.Parameters.AddWithValue("@password", password);

                try
                {
                    conn.Open();
                    int exists = (int)checkCmd.ExecuteScalar();

                    if (exists > 0)
                    {
                        // username or email exists
                        lblMessage.Text = "Username or Email already exists.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        insertCmd.ExecuteNonQuery();
                        String message = "Account successfully created.";
                        Session["account-created"] = message;
                        Response.Redirect("LoginPage.aspx");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                }
            }
        }

        // for password hashing
        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));  // convert to hex string
                }
                return builder.ToString();
            }
        }
    }
}