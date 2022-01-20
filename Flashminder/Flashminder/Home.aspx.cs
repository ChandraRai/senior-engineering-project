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
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // If loading the page for the first time, populate the UserProfileGridView
            if(!IsPostBack)
            {
                // Get the UserProfile data
                this.GetUserProfile();

            }
        }

        // Method to get user profile data from DB
        protected void GetUserProfile()
        {
            // Connect to EF
            using (DefaultConnection db = new DefaultConnection())
            {
                // query USERS table using EF and LINQ
                var Users = (from allUsers in db.USERS select allUsers);

                // Bind the result to Gridview
                //UserProfileGridView.DataSource = Users.ToList();
                //UserProfileGridView.DataBind();
                rptrUserProfile.DataSource = Users.ToList();
                rptrUserProfile.DataBind();
            }
        }
    }
}