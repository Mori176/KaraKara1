using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiniProject_Karakara
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] != null)
                {
                    string role = Session["role"].ToString();
                    navHide.Visible = true;
                    btnAuth.Text = "Sign Out";

                    if (role == "admin")
                    {
                        adminLinks.Visible = true;
                        customerLinks.Visible = false;
                    }
                    else if (role == "customer")
                    {
                        adminLinks.Visible = false;
                        customerLinks.Visible = true;
                    }
                    else
                    {
                        adminLinks.Visible = false;
                        customerLinks.Visible = false;
                    }
                }
                else
                {
                    navHide.Visible = false;
                    btnAuth.Text = "Sign In";
                }
            }
        }

        protected void btnAuth_Click(object sender, EventArgs e)
        {
            if(Session["username"] != null)
            {
                Session.Abandon();
                Response.Redirect("LoginPage.aspx");
            }
            else
            {
                Response.Redirect("LoginPage.aspx");
            }
        }
    }
}