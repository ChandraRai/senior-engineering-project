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
                if(HttpContext.Current.Session["CurrentUser"] == null)
                {
                    Response.Redirect("About.aspx");
                }
                else
                {
                    // Get the UserProfile data
                    this.GetUserProfile();
                }
                
            }
        }

        // Method to get user profile data from DB
        protected void GetUserProfile()
        {
            // Bind the result to Gridview 
            rptrUserProfile.DataSource = GetUser();
            rptrUserProfile.DataBind();
        }

        protected List<USERS> GetUser()
        {          
            var myUser = new List<USERS>();

            // Connect to EF
            using (DefaultConnection db = new DefaultConnection())
            {
                // query USERS table using EF and LINQ
                var Users = (from allUsers in db.USERS select allUsers);

                foreach (var user in Users)
                {
                    if (HttpContext.Current.Session["CurrentUser"] is null)
                    {
                        Session["CurrentUser"] = "";
                        Session["Email"] = "";
                        System.Web.Security.FormsAuthentication.SetAuthCookie("", false);
                    }

                    // verify Username
                    if (user.Username == HttpContext.Current.Session["CurrentUser"].ToString())
                    {
                        myUser.Add(user);
                        break;
                    }
                }
                return myUser;
            }
        }

        protected void StartQuiz_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ViewQuiz.aspx");
        }
    }
}