using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

// Using statements that are required to connect to EF DB
using Flashminder.Models;
using System.Web.ModelBinding;
using System.Reflection;

namespace Flashminder
{
    public enum WarningType
    {
        Success,
        Info,
        Warning,
        Danger
    }

    public partial class CreateFlashcard : System.Web.UI.Page
    {
        const int MAX_CARD_TEXT = 500;

        //https://stackoverflow.com/questions/35823379/bootstrap-alert-in-button-event-on-asp-net
        public void ShowMessage(string message, WarningType warning)
        {
            message_lbl.Text = message;
            message_pnl.CssClass = string.Format("alert alert-{0} alert-dismissable", warning.ToString().ToLower());
            message_pnl.Attributes.Add("role", "alert");
            message_pnl.Visible = true;
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            string user = HttpContext.Current.User.Identity.Name;
            if (string.IsNullOrEmpty(user))
            {
                Response.Redirect("~/Signin.aspx");
            }
            if ((Session["ResultMessage"] != null) && (Session["ResultType"] != null))
            {
                string msg = Session["ResultMessage"].ToString();
                WarningType status = WarningType.Info;
                Enum.TryParse(Session["ResultType"].ToString(), out status);
                if (!string.IsNullOrEmpty(msg))
                {
                    ShowMessage(msg, status);
                }
                Session.Remove("ResultMessage");
                Session.Remove("ResultType");
            }

        }

        private bool ValidateForm()
        {
            if ((string.IsNullOrEmpty(front_txtbx.Text) && string.IsNullOrEmpty(front_upload.FileName)) ||
                (string.IsNullOrEmpty(back_txtbx.Text) && string.IsNullOrEmpty(back_upload.FileName)))
            {
                ShowMessage("Each card side needs text or an image.", WarningType.Warning);
                return false;
            }
            if (front_txtbx.Text.Length > MAX_CARD_TEXT || back_txtbx.Text.Length > MAX_CARD_TEXT)
            {
                ShowMessage("Max text length is 500 characters", WarningType.Warning);
                return false;
            }
            return true;
        }

        protected void CreateFlashCard(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                return;
            }
            string user = HttpContext.Current.User.Identity.Name;
            if (!string.IsNullOrEmpty(user))
            {
                // Connect to EF
                using (DefaultConnection db = new DefaultConnection())
                {
                    Flashcard flashcard = new Flashcard();
                    flashcard.CardType = db.CardTypes.Find(1);
                    flashcard.UserId = int.Parse(user);
                    if (front_txtbx.Text.Length <= MAX_CARD_TEXT)
                    {
                        flashcard.FrontText = front_txtbx.Text; // clean?
                    }
                    if (back_txtbx.Text.Length <= MAX_CARD_TEXT)
                    {
                        flashcard.BackText = back_txtbx.Text;
                    }
                    if (front_upload.FileBytes != null)
                    {
                        flashcard.FrontImage = front_upload.FileBytes;
                    }
                    if (back_upload.FileBytes != null)
                    {
                        flashcard.BackImage = back_upload.FileBytes;
                    }
                    flashcard.CreatedDate = DateTime.Now;
                    db.Flashcards.Add(flashcard);
                    int ret = db.SaveChanges();
                    if (ret > 0)
                    {
                        Session.Add("ResultMessage", "Successfully created flashcard");
                        Session.Add("ResultType", "Sucess");
                        Response.Redirect(Request.Url.AbsoluteUri);
                    }
                    else
                    {
                        ShowMessage("Unable to create flashcard", WarningType.Warning);
                    }
                }
            }
             else
            {
                ShowMessage("User is not logged in.", WarningType.Danger);
            }
        }
    }
}