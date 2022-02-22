using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Flashminder.Models;

namespace Flashminder
{
    public class DatabaseMutators
    {
        static public int CreateDefaultCategory(int userId)
        {
            int ret = 0;
            using (DefaultConnection db = new DefaultConnection())
            {
                Category category = new Category();
                category.CategoryName = "Default";
                category.CreatedDate = DateTime.Now;
                category.UserId = userId;
                db.Categories.Add(category);
                ret = db.SaveChanges();
            }
            return ret;
        }

        static public int UpdateNextDate(Flashcard_Algorithm_Data data, float multiplier)
        {
            int ret = 0;
            using (DefaultConnection db = new DefaultConnection())
            {
                Flashcard_Algorithm_Data update = db.Flashcard_Algorithm_Data.Find(data.Id);
                Flashcard_Algorithm_Data temp = LearningAlgorithms.UpdateSM2Algorithm(data, multiplier);
                update.Easiness = temp.Easiness;
                update.Interval = temp.Interval;
                update.Repetitions = temp.Repetitions;
                update.Quality = temp.Quality;
                update.NextPratice = temp.NextPratice;
                ret = db.SaveChanges();
            }
            return ret;
        }
    }
}