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

        //show message on client
        //https://stackoverflow.com/questions/35823379/bootstrap-alert-in-button-event-on-asp-net
        public void ShowMessage(string message, WarningType warning)
        {
            message_lbl.Text = message;
            message_pnl.CssClass = string.Format("alert alert-{0} alert-dismissable", warning.ToString().ToLower());
            message_pnl.Attributes.Add("role", "alert");
            message_pnl.Visible = true;
        }


        // on page load
        protected void Page_Load(object sender, EventArgs e)
        {
            string user = HttpContext.Current.User.Identity.Name;
            if (string.IsNullOrEmpty(user))
            {
                Response.Redirect("~/Signin.aspx");
            }

            // if no category exists, make a default category
            // populate drop down
            using (DefaultConnection db = new DefaultConnection())
            {
                int userInt = int.Parse(user);
                List<Category>categories = db.Categories.Where(cat=>(cat.UserId==userInt)).ToList();
                if (categories.Count == 0)
                {
                    DatabaseMutators.CreateDefaultCategory(userInt);
                }
                category_dropdownlist.DataSource = db.Categories.Where(cat => (cat.UserId == userInt)).ToList();
                category_dropdownlist.DataBind();
            }
            
            // set result message if successful then clear
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

        // validate the flashcard has valid information
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

        // create the flashcard with given information
        protected void CreateFlashCard(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                return;
            }
            string user = HttpContext.Current.User.Identity.Name;
            int userInt = int.Parse(user);
            if (!string.IsNullOrEmpty(user))
            {
                // Connect to EF
                using (DefaultConnection db = new DefaultConnection())
                {
                    Flashcard flashcard = new Flashcard();
                    Flashcard_Category relation = new Flashcard_Category(); // set flashcard 2 category too
                    Flashcard_Algorithm_Data algData = new Flashcard_Algorithm_Data();
                    algData.Easiness = 2.5;
                    algData.Flashcard = flashcard;
                    algData.Interval = 1;
                    algData.Quality = 0;
                    algData.Repetitions = 0;
                    algData.NextPratice = DateTime.Now;

                    relation.Flashcard = flashcard;
                    relation.CategoryId = int.Parse(category_dropdownlist.SelectedValue);
                    relation.UserID = userInt;
                    flashcard.CardType = db.CardTypes.Find(1); // set to default right now, set for different types later
                    flashcard.UserId = userInt;
                    flashcard.FrontImage = "";
                    flashcard.BackImage = "";
                    if (front_txtbx.Text.Length <= MAX_CARD_TEXT)
                    {
                        flashcard.FrontText = front_txtbx.Text; // clean?
                    }
                    if (back_txtbx.Text.Length <= MAX_CARD_TEXT)
                    {
                        flashcard.BackText = back_txtbx.Text;
                    }
                    if (!string.IsNullOrEmpty(front_upload.FileName))
                    {
                        string fileName = DateTime.Now.ToString("MM-dd-yyyy_HHmmss");
                        string filetype = Path.GetExtension(front_upload.FileName).ToString().ToLower();
                        front_upload.SaveAs(Server.MapPath("Images/" + user + "_" + fileName + "_front_" + filetype));
                        flashcard.FrontImage = user + "_" + fileName + "_front_" + filetype;
                    }
                    if (!string.IsNullOrEmpty(back_upload.FileName))
                    {
                        string fileName = DateTime.Now.ToString("MM-dd-yyyy_HHmmss");
                        string filetype = Path.GetExtension(back_upload.FileName).ToString().ToLower();
                        back_upload.SaveAs(Server.MapPath("Images/" + user + "_" + fileName + "_back_" + filetype));
                        flashcard.BackImage = user + "_" + fileName + "_back_" + filetype;
                    }
                    flashcard.CreatedDate = DateTime.Now;
                    db.Flashcards.Add(flashcard);
                    db.Flashcard_Category.Add(relation);
                    db.Flashcard_Algorithm_Data.Add(algData);
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