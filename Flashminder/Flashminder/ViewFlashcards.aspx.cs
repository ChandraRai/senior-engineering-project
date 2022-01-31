using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Flashminder.Models;

namespace Flashminder
{
    public partial class ViewFlashcards : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string user = HttpContext.Current.User.Identity.Name;
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(user))
                {
                    int userInt = Int32.Parse(user);
                    using (DefaultConnection db = new DefaultConnection())
                    {
                        category_dropdownlist.DataSource = db.Categories.Where(cat => cat.UserId == userInt).ToList();
                        category_dropdownlist.DataBind();
                    }            
                    LoadFlashcards(user, category_dropdownlist.SelectedValue);
                }
            }
        }

        protected void SwitchCategory(object sender, EventArgs e)
        {
            LoadFlashcards(HttpContext.Current.User.Identity.Name, category_dropdownlist.SelectedItem.Text);
        }

        private void LoadFlashcards(string user, string category)
        {
            if (!string.IsNullOrEmpty(user))
            {                
                int userInt = Int32.Parse(user);

                using (DefaultConnection db = new DefaultConnection())
                {
                    if (category_dropdownlist.SelectedValue=="All")
                    {

                        flashcards_datalist.DataSource = db.Flashcards.Where(flashcard => (flashcard.UserId == userInt)).ToList();
                    }
                    else
                    {
                        IQueryable<Flashcard_Category> flashcardCategory = db.Flashcard_Category.Where(cat=>(cat.Category.CategoryName == category));
                        List<Flashcard> cards = (from flashcards in db.Flashcards join combined in flashcardCategory on flashcards.Id equals combined.FlashcardId where flashcards.UserId == userInt select flashcards).ToList();
                        flashcards_datalist.DataSource = cards;
                    }
                    flashcards_datalist.DataBind();
                }
            }
        }
    }
}