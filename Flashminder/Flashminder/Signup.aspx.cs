﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Flashminder
{
    public partial class Signup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // WebForms UnobtrusiveValidationMode requires a ScriptResourceMapping for jquery
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        }
    }
}