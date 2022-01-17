﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Flashminder.Custom_User_Controls
{
    public partial class Navbar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageActive();
        }
        /**
         * 
         * This method adds CSS class active to navigation links.
         * @method SetPageActive
         * @return void
         */
        private void SetPageActive()
        {
            switch (Page.Title)
            {
                case "Sign in":
                    signin.Attributes.Add("class", "active");
                    break;
                case "Sign up":
                    signup.Attributes.Add("class", "active");
                    break;
                case "About":
                    about.Attributes.Add("class", "active");
                    break;
            }
        }

    }
}