using System;
using System.Web.UI;

namespace MiniProject_Karakara
{
    // Design Pattern 5: Template Method
    // Defines the skeleton of the Page_Load flow, letting subclasses 
    // override specific steps (OnLoadLogic) without changing the overall structure.
    public abstract class BasePage : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                // 1. Common Pre-load Logic (e.g., logging)
                
                // 2. Authentication Check (if required by subclass)
                if (RequiresAuthentication && Session["username"] == null)
                {
                    Response.Redirect("LoginPage.aspx");
                    return;
                }

                // 3. Execution of the actual page logic
                base.OnLoad(e);
                OnLoadLogic(e);
            }
            catch (Exception ex)
            {
                // 4. Global Exception Handling
                HandleError(ex);
            }
        }

        // Hook method for subclasses to implement their specific logic
        protected virtual void OnLoadLogic(EventArgs e) { }

        // Hook to specify if page needs login
        protected virtual bool RequiresAuthentication => false;

        protected void HandleError(Exception ex)
        {
            // Log error (simulated)
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            
            // Show user-friendly message if a Label named 'lblMessage' exists
            var lbl = FindControl("lblMessage") as System.Web.UI.WebControls.Label;
            if (lbl != null)
            {
                lbl.Text = "An error occurred: " + ex.Message;
                lbl.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
