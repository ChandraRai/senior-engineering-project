using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Configuration;
using NUnit.Framework;
using Flashminder.Models;
using OpenQA.Selenium.Support.UI;
using System.Drawing;

namespace FlashminderTest
{
    class ViewFlashcardTest
    {
        string url = "";
        string serverPath = "";
        string imagePath = "";
        WebDriver driver;
        int userId = 0;

        [SetUp]
        public void StartBrowser()
        {
            url = ConfigurationManager.AppSettings.Get("Website");
            imagePath = ConfigurationManager.AppSettings.Get("ImagesPath");
            serverPath = ConfigurationManager.AppSettings.Get("ServerPath");
            driver = new ChromeDriver("./");
            userId = Helper.CreateUser("Test1", "Test");
        }

        public void GoToViewFlashcards()
        {
            Helper.SignIn(driver, url, "Test1", "Test");
            driver.Navigate().GoToUrl(url + "/ViewFlashcards.aspx");
        }

        [Test]
        public void TestTextFlashcard()
        {
            Helper.CreateCategory(userId, "Custom", "Custom Description");
            Helper.CreateFlashcardText(userId, "Custom", "Front", "Back");
            GoToViewFlashcards();
            var flashcards = driver.FindElements(By.ClassName("flashcard"));
            int correct = 0;
            foreach(WebElement card in flashcards)
            {
                if (card.Text.Contains("Front"))
                {
                    correct++;
                }
            }
            Assert.IsTrue(correct >= 1);
        }

        [Test]
        public void TestImageFlashcard()
        {
            Helper.CreateCategory(userId, "Custom", "Custom Description");
            Image front = Image.FromFile(imagePath + "\\front_test.png");
            Image back = Image.FromFile(imagePath + "\\back_test.png");
            Helper.CreateFlashcardWithImages(userId, "Custom", "", "", front, back);
            GoToViewFlashcards();
            var flashcards = driver.FindElements(By.ClassName("flashcard"));
            int correct = 0;
            foreach (WebElement card in flashcards)
            {
                if (card.Displayed)
                {
                    correct++;
                }
            }
            Assert.IsTrue(correct >= 1);
        }

        [Test]
        public void TestMultipleFlashcards()
        {
            Helper.CreateCategory(userId, "Custom", "Custom Description");
            Helper.CreateFlashcardText(userId, "Custom", "Front1", "Back");
            Helper.CreateFlashcardText(userId, "Custom", "Front2", "Back");
            Helper.CreateFlashcardText(userId, "Custom", "Front3", "Back");
            Helper.CreateFlashcardText(userId, "Custom", "Front4", "Back");

            GoToViewFlashcards();
            var flashcards = driver.FindElements(By.ClassName("flashcard"));
            Assert.IsTrue(flashcards.Count == 4);
        }

        [Test]
        public void TestDeleteFlashcard()
        {
            Helper.CreateCategory(userId, "Custom", "Custom Description");
            Helper.CreateFlashcardText(userId, "Custom", "Front", "Back");
            GoToViewFlashcards();
            var flashcards = driver.FindElements(By.ClassName("flashcard"));

            var delete = flashcards[0].FindElement(By.Id("delete_btn"));
            delete.Click();
            driver.SwitchTo().Alert().Accept();
            GoToViewFlashcards();
            flashcards = driver.FindElements(By.ClassName("flashcard"));

            Assert.IsTrue(flashcards.Count == 0);
        }


        [Test]
        public void TestDeleteCancelFlashcard()
        {
            Helper.CreateCategory(userId, "Custom", "Custom Description");
            Helper.CreateFlashcardText(userId, "Custom", "Front", "Back");
            GoToViewFlashcards();
            var flashcards = driver.FindElements(By.ClassName("flashcard"));

            var delete = flashcards[0].FindElement(By.Id("delete_btn"));
            delete.Click();
            driver.SwitchTo().Alert().Dismiss();
            GoToViewFlashcards();
            flashcards = driver.FindElements(By.ClassName("flashcard"));

            Assert.IsTrue(flashcards.Count == 1);
        }

        [TearDown]
        public void CloseBrowser()
        {
            if (driver != null)
            {
                driver.Close();
                driver.Quit();
            }

            Helper.RemoveUser(userId);
        }
    }
}
