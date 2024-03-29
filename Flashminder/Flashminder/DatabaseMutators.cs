﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;


using Flashminder.Models;

namespace Flashminder
{
    public class DatabaseMutators
    {
        const int MAX_CARD_TEXT = 500;

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

        static public int CreateFlashcard(int userID, string categoryName, string frontText, string backText, string frontImage, string backImage)
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
                algData.UserId = userID;

                relation.Flashcard = flashcard;
                int selectedCategory = db.Categories.Where(cat => (cat.CategoryName == categoryName)).FirstOrDefault().Id;
                relation.CategoryId = selectedCategory;
                relation.UserID = userID;
                flashcard.UserId = userID;
                flashcard.FrontImage = "";
                flashcard.BackImage = "";
                if (frontText.Length<= MAX_CARD_TEXT)
                {
                    flashcard.FrontText = frontText; // clean?
                }
                if (backText.Length <= MAX_CARD_TEXT)
                {
                    flashcard.BackText = backText;
                }
                flashcard.FrontImage = frontImage;
                flashcard.BackImage = backImage;
                flashcard.CreatedDate = DateTime.Now;
                db.Flashcards.Add(flashcard);
                db.Flashcard_Category.Add(relation);
                db.Flashcard_Algorithm_Data.Add(algData);
                int ret = db.SaveChanges();
                return ret;
            }
        }

        static public int CreateFlashcard(int userID, string categoryName, string frontText, string backText, Image frontImage, Image backImage)
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
                algData.UserId = userID;

                relation.Flashcard = flashcard;
                int selectedCategory = db.Categories.Where(cat => (cat.CategoryName == categoryName)).FirstOrDefault().Id;
                relation.CategoryId = selectedCategory;
                relation.UserID = userID;
                flashcard.UserId = userID;
                flashcard.FrontImage = "";
                flashcard.BackImage = "";
                if (frontText.Length <= MAX_CARD_TEXT)
                {
                    flashcard.FrontText = frontText; // clean?
                }
                if (backText.Length <= MAX_CARD_TEXT)
                {
                    flashcard.BackText = backText;
                }

                if (frontImage != null)
                {
                    string fileName = DateTime.Now.ToString("MM-dd-yyyy_HHmmss");
                    string filetype = "." + frontImage.RawFormat.ToString().ToLower();
                    var blobClient = BloblUtil.GetBlobContainer().GetBlobClient(userID + "_" + fileName + "_front_" + filetype);
                    MemoryStream ms = new MemoryStream();
                    frontImage.Save(ms, frontImage.RawFormat);
                    ms.Position = 0;
                    using (var stream = ms)
                    {
                        blobClient.Upload(stream);
                    }
                    flashcard.FrontImage = userID + "_" + fileName + "_front_" + filetype;
                }
                if (backImage != null)
                {
                    string fileName = DateTime.Now.ToString("MM-dd-yyyy_HHmmss");
                    string filetype = "." + backImage.RawFormat.ToString().ToLower();
                    var blobClient = BloblUtil.GetBlobContainer().GetBlobClient(userID + "_" + fileName + "_back_" + filetype);
                    MemoryStream ms = new MemoryStream();
                    backImage.Save(ms, frontImage.RawFormat);
                    ms.Position = 0;
                    using (var stream = ms)
                    {
                        blobClient.Upload(stream);
                    }
                    flashcard.BackImage = userID + "_" + fileName + "_back_" + filetype;
                }

                flashcard.CreatedDate = DateTime.Now;
                db.Flashcards.Add(flashcard);
                db.Flashcard_Category.Add(relation);
                db.Flashcard_Algorithm_Data.Add(algData);
                int ret = db.SaveChanges();
                return ret;
            }
        }

        static public bool DeleteFlashcard(int id)
        {
            bool success = false;
            using (DefaultConnection db = new DefaultConnection())
            {
                Flashcard toRemove = db.Flashcards.Where(card => (card.Id == id)).FirstOrDefault();
                Flashcard_Category relation = db.Flashcard_Category.Where(cat => (cat.FlashcardId == id)).FirstOrDefault();
                Flashcard_Algorithm_Data alg = db.Flashcard_Algorithm_Data.Where(algData => (algData.FlashcardId == id)).FirstOrDefault();
                if (toRemove != null)
                {
                    db.Flashcards.Remove(toRemove);
                    db.Flashcard_Category.Remove(relation);
                    db.Flashcard_Algorithm_Data.Remove(alg);
                }

                if (db.SaveChanges() == 3)
                {
                    if (!string.IsNullOrEmpty(toRemove.FrontImage))
                    {
                        var blobClient = BloblUtil.GetBlobContainer().GetBlobClient(toRemove.FrontImage);
                        blobClient.Delete();
                    }
                    if(!string.IsNullOrEmpty(toRemove.BackImage))
                    {
                        var blobClient = BloblUtil.GetBlobContainer().GetBlobClient(toRemove.BackImage);
                        blobClient.Delete();
                    }
                    success = true;
                }
            }
            return success;
        }

    }
}