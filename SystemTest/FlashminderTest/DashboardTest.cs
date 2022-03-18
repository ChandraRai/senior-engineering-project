using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using System.Configuration;
using NUnit.Framework;

namespace FlashminderTest
{
    class DashboardTest
    {
        string url = "";
        WebDriver driver;


        [SetUp]
        public void StartBrowser()
        {
            url = ConfigurationManager.AppSettings.Get("Website");
            driver = new ChromeDriver("./");
            Helper.CreateUser("Test1", "Test");
        }

        public bool GoToDashboard()
        {
            Helper.SignIn(driver, url, "Test1", "Test");
            driver.Navigate().GoToUrl(url + "/ViewDashboard.aspx");
            return driver.Title == "Dashboard Page";
        }

        [Test]
        public void TestCreateFlashcardsButton()
        {
            GoToDashboard();
            var createFlashcardButton = driver.FindElement(By.Id("MainContent_CreateFlashcards"));
            createFlashcardButton.Click();
            Assert.IsTrue(driver.Title == "Create Flashcard");
        }

        [Test]
        public void TestViewFlashcardsButton()
        {
            GoToDashboard();
            var viewFlashcardButton = driver.FindElement(By.Id("MainContent_ViewFlashcards"));
            viewFlashcardButton.Click();
            Assert.IsTrue(driver.Title == "View Flashcards");
        }

        public void TestStartQuizButton()
        {
            GoToDashboard();
            var startQuizButton = driver.FindElement(By.Id("MainContent_startquiz_btn"));
            startQuizButton.Click();
            Assert.IsTrue(driver.Title == "Quiz");
        }

        public void TestDifferentCategory()
        {
            GoToDashboard();
            var startQuizButton = driver.FindElement(By.Id("MainContent_startquiz_btn"));
            SelectElement dropdown = new SelectElement(driver.FindElement(By.Id("MainContent_category_dropdownlist")));
            dropdown.SelectByText("Default");
            startQuizButton.Click();
            var category = driver.FindElement(By.Id("MainContent_category"));
            Assert.IsTrue(category.Text == "Default");
        }

        [TearDown]
        public void CloseBrowser()
        {
            if (driver != null)
            {
                driver.Close();
                driver.Quit();
            }

            Helper.RemoveUser("Test1");
        }
    }
}
