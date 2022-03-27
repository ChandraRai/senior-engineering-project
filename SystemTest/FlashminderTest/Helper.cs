using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

using System.Web.UI;
using Flashminder;
using Flashminder.Models;
using System.Security.Cryptography;
using System.Drawing;
using OpenQA.Selenium.Support.UI;

namespace FlashminderTest
{
    class Helper
    {
        
        static public int CreateUser(string username, string password)
        {
            USERS user = new USERS();
            user.Username = username;
            user.Password = Sha1(Salt(password));
            user.Email = "test@test.com";
            user.Privilege = 1;
            using (DefaultConnection db = new DefaultConnection())
            {
                db.USERS.Add(user);
                db.SaveChanges();
            }
            return user.Id;
        }

        public static string Sha1(string text)
        {
            byte[] clear = System.Text.Encoding.UTF8.GetBytes(text);
            byte[] hash = new SHA1CryptoServiceProvider().ComputeHash(clear);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        // Password Encryption        
        public static string Salt(string text)
        {
            return
              "zu5QnKrH4NJfOgV2WWqV5Oc1l" +
              text +
              "1DMuByokGSDyFPQ0DbXd9rAgW";
        }

        static public bool RemoveUser(string userToRemove)
        {
            int changes = 0;
            using (DefaultConnection db = new DefaultConnection())
            {
                USERS user1 = db.USERS.Where(user => (user.Username == userToRemove)).FirstOrDefault();
                if (user1 != null)
                {
                    foreach (Category cat in user1.Categories)
                    {
                        db.Categories.Remove(cat);
                    }

                    foreach (Flashcard card in user1.Flashcards)
                    {
                        foreach (Flashcard_Algorithm_Data data in card.Flashcard_Algorithm_Data)
                        {
                            db.Flashcard_Algorithm_Data.Remove(data);
                        }
                        db.Flashcards.Remove(card);
                    }

                    foreach (Flashcard_Category flashCat in user1.Flashcard_Category)
                    {
                        db.Flashcard_Category.Remove(flashCat);
                    }

                    db.USERS.Remove(user1);
                    changes = db.SaveChanges();
                }
            }
            if (changes >0)
            {
                return true;
            }
            return false;
        }

        static public bool RemoveUser(int ID)
        {
            int changes = 0;
            using (DefaultConnection db = new DefaultConnection())
            {
                USERS user1 = db.USERS.Where(user => (user.Id == ID)).FirstOrDefault();
                if (user1 != null)
                {
                    foreach (Category cat in user1.Categories)
                    {
                        db.Categories.Remove(cat);
                    }

                    foreach (Flashcard card in user1.Flashcards)
                    {
                        foreach (Flashcard_Algorithm_Data data in card.Flashcard_Algorithm_Data)
                        {
                            db.Flashcard_Algorithm_Data.Remove(data);
                        }

                        if (!string.IsNullOrEmpty(card.FrontImage))
                        {
                            var blobClient = BloblUtil.GetBlobContainer().GetBlobClient(card.FrontImage);
                            blobClient.Delete();
                        }
                        if (!string.IsNullOrEmpty(card.BackImage))
                        {
                            var blobClient = BloblUtil.GetBlobContainer().GetBlobClient(card.BackImage);
                            blobClient.Delete();
                        }

                        db.Flashcards.Remove(card);
                    }

                    foreach (Flashcard_Category flashCat in user1.Flashcard_Category)
                    {
                        db.Flashcard_Category.Remove(flashCat);
                    }

                    db.USERS.Remove(user1);
                    changes = db.SaveChanges();
                }
            }
            if (changes > 0)
            {
                return true;
            }
            return false;
        }


        static public bool SignIn(WebDriver driver, string url, string username, string password) 
        {
            bool success = false;
            if (driver != null)
            {
                driver.Navigate().GoToUrl(url + "/Signin.aspx");
                var usernameBox = driver.FindElement(By.Id("MainContent_UserNameTextBox"));
                var passwordBox = driver.FindElement(By.Id("MainContent_PasswordTextBox"));
                var sendButton = driver.FindElement(By.Id("MainContent_SendButton"));

                usernameBox.SendKeys(username);
                passwordBox.SendKeys(password);
                sendButton.Click();
                if (driver.Title == "Home Page")
                {
                    success = true;
                }
            }
            return success;
        }

        static public bool CreateFlashcardText(int userId, string category, string front, string back) 
        {
            return DatabaseMutators.CreateFlashcard(userId, category, front, back, "", "") > 0;
        }

        static public bool CreateFlashcardWithImages(int userId, string category, string front, string back, Image frontImg, Image backImg)
        {
            return DatabaseMutators.CreateFlashcard(userId, category, front, back, frontImg, backImg) > 0;
        }

        static public bool CreateCategory(int userID, string category, string categoryDesc)
        {
            Category cat = new Category();
            int changes = 0;
            cat.CategoryName = category;
            cat.CategoryDesc = categoryDesc;
            cat.CreatedDate = DateTime.Now;
            cat.UserId = userID;
            using (DefaultConnection db = new DefaultConnection())
            {
                db.Categories.Add(cat);
                changes = db.SaveChanges();
            }
            return changes > 0;
        }

    }
}
