using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Flashminder.Models;

namespace Flashminder
{
    public partial class ViewQuiz : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string userId = HttpContext.Current.User.Identity.Name;
            if (string.IsNullOrEmpty(userId))
            {
                Response.Redirect("~/Signin.aspx");
            }

            if (!IsPostBack)
            {
                string categoryName = Request.QueryString["CategoryName"];
                category.Text = categoryName;
                if (string.IsNullOrEmpty(category.Text))
                {
                    category.Text = "All";
                }
            }                
            LoadFlashcard(userId, category.Text);
            SetButtons();
        }


        protected Flashcard LoadFlashcard(string userId, string categoryName)
        {
            List<Flashcard> flashcards = DatabaseAccessors.LoadFlashcardsByCategoryName(userId, categoryName);
            List<Flashcard_Algorithm_Data> data = null;
            Flashcard ret = null;
            if (flashcards != null)
            {
                data = DatabaseAccessors.LoadNextFlashcardData(flashcards);
            }

            if ( data != null)
            {
                Flashcard_Algorithm_Data dataObj = data.FirstOrDefault();
                Flashcard card = null;
                if (dataObj != null)
                {
                    card = DatabaseAccessors.LoadFlashcard((int)dataObj.FlashcardId, Int32.Parse(userId));
                    dataObj.Flashcard = null;
                    Session["curData"] = dataObj;
                }
                if (card != null)
                {
                    currentFlashcardId_label.Text = card.Id.ToString();
                    ret = card;
                }
            }

            return ret;
        }

        protected void SetButtons()
        {
            // set very easy
            float multiplier = float.Parse(multiplier_dropdown.SelectedValue);
            Flashcard_Algorithm_Data curData =  (Flashcard_Algorithm_Data)Session["curData"];
            if (curData != null)
            {
                TimeSpan daysDateTime = (LearningAlgorithms.CalculateSM2Alg(5, curData.Easiness, curData.Interval, curData.Repetitions, multiplier) - DateTime.Now);
                double time = daysDateTime.TotalDays > 1 ? daysDateTime.TotalDays : daysDateTime.TotalHours;
                string timeString = daysDateTime.TotalDays > 1 ? " days" : " hours";
                very_easy_btn.Text = "Very Easy (" + time.ToString("0.0") + timeString + ")";
                daysDateTime = (LearningAlgorithms.CalculateSM2Alg(4, curData.Easiness, curData.Interval, curData.Repetitions, multiplier) - DateTime.Now);
                time = daysDateTime.TotalDays > 1 ? daysDateTime.TotalDays : daysDateTime.TotalHours;
                timeString = daysDateTime.TotalDays > 1 ? " days" : " hours";
                easy_btn.Text = "Easy (" + time.ToString("0.0") + timeString + ")";
                daysDateTime = (LearningAlgorithms.CalculateSM2Alg(3, curData.Easiness, curData.Interval, curData.Repetitions, multiplier) - DateTime.Now);
                time = daysDateTime.TotalDays > 1 ? daysDateTime.TotalDays : daysDateTime.TotalHours;
                timeString = daysDateTime.TotalDays > 1 ? " days" : " hours";
                moderate_btn.Text = "Moderate ("+ time.ToString("0.0") + timeString + ")";
                daysDateTime = (LearningAlgorithms.CalculateSM2Alg(2, curData.Easiness, curData.Interval, curData.Repetitions, multiplier) - DateTime.Now);
                time = daysDateTime.TotalDays > 1 ? daysDateTime.TotalDays : daysDateTime.TotalHours;
                timeString = daysDateTime.TotalDays > 1 ? " days" : " hours";
                hard_btn.Text = "Hard (" + time.ToString("0.0") + timeString + ")";
                daysDateTime = (LearningAlgorithms.CalculateSM2Alg(1, curData.Easiness, curData.Interval, curData.Repetitions, multiplier) - DateTime.Now);
                time = daysDateTime.TotalDays > 1 ? daysDateTime.TotalDays : daysDateTime.TotalHours;
                timeString = daysDateTime.TotalDays > 1 ? " days" : " hours";
                very_hard_btn.Text = "Very Hard (" + time.ToString("0.0") + timeString + ")";
            }
        }

        protected void ButtonPressed( object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Flashcard_Algorithm_Data curData = (Flashcard_Algorithm_Data) Session["curData"];

            if (curData != null)
            {
                if (btn.Text.StartsWith("Very Easy"))
                {
                    curData.Quality = 5;
                    DatabaseMutators.UpdateNextDate(curData, float.Parse(multiplier_dropdown.SelectedValue));
                }
                else if (btn.Text.StartsWith("Easy"))
                {
                    curData.Quality = 4;
                    DatabaseMutators.UpdateNextDate(curData, float.Parse(multiplier_dropdown.SelectedValue));
                }
                else if (btn.Text.StartsWith("Moderate"))
                {
                    curData.Quality = 3;
                    DatabaseMutators.UpdateNextDate(curData, float.Parse(multiplier_dropdown.SelectedValue));
                }
                else if (btn.Text.StartsWith("Hard"))
                {
                    curData.Quality = 2;
                    DatabaseMutators.UpdateNextDate(curData, float.Parse(multiplier_dropdown.SelectedValue));
                }
                else if (btn.Text.StartsWith("Very Hard"))
                {
                    curData.Quality = 1;
                    DatabaseMutators.UpdateNextDate(curData, float.Parse(multiplier_dropdown.SelectedValue));
                }
            }
            string userId = HttpContext.Current.User.Identity.Name;

            LoadFlashcard(userId, category.Text);
        }

        protected void DropdownChanged(object sender, EventArgs e)
        {
            SetButtons();
        }
    }
}