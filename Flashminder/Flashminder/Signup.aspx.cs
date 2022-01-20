using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// Using statements that are required to connect to EF DB
using Flashminder.Models;
using System.Web.ModelBinding;

namespace Flashminder
{
    public partial class Signup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // WebForms UnobtrusiveValidationMode requires a ScriptResourceMapping for jquery
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        }

        protected void RegisterButton_Click(object sender, EventArgs e)
        {
            // Connect to EF
            using (DefaultConnection db = new DefaultConnection())
            {
                // Use Flashminder to  create a new user object and save a new record
                USERS newUser = new USERS();

                // add data to record
                newUser.Username = txtUsername.Text;
                newUser.Email = txtEmail.Text;
                newUser.Password = txtPassword.Text;

                // user LINQ to ADO.NET to save record
                db.USERS.Add(newUser);

                // Save
                db.SaveChanges();

                // Redirect back to Login Page
                Response.Redirect("~/Signin.aspx");
            }

        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            // Redirect to About Page
            Response.Redirect("~/About.aspx");
        }
    }
}