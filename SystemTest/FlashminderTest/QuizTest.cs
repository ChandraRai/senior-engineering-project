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

namespace FlashminderTest
{
    class QuizTest
    {
        string url = "";
        WebDriver driver;
        int userId = 0;

        [SetUp]
        public void StartBrowser()
        {
            url = ConfigurationManager.AppSettings.Get("Website");
            driver = new ChromeDriver("./");
            userId = Helper.CreateUser("Test1", "Test");
        }

        public void GoToQuiz(string category)
        {
            Helper.SignIn(driver, url, "Test1", "Test");
            driver.Navigate().GoToUrl(url + "/ViewQuiz.aspx" +"?CategoryName="+category);
        }
        [Test]
        public void TestSingleFlashcards()
        {
            Helper.CreateCategory(userId, "Custom", "Custom Description");
            Helper.CreateFlashcardText(userId, "Custom", "Front1", "Back");

            GoToQuiz("Custom");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            IWebElement front = driver.FindElement(By.ClassName("front-text"));
            IWebElement back = driver.FindElement(By.ClassName("back-text"));
            Assert.IsTrue(front.Text.Contains("Front1"));
            Assert.IsTrue(back.Text.Contains("Back"));
            Assert.IsTrue(front.Displayed);
            var card = driver.FindElement(By.ClassName("flip-card"));
            card.Click();
            Assert.IsTrue(back.Displayed);
        }

        [Test]
        public void TestThreeFlashcards()
        {
            Helper.CreateCategory(userId, "Custom", "Custom Description");
            Helper.CreateFlashcardText(userId, "Custom", "Front1", "Back");
            Helper.CreateFlashcardText(userId, "Custom", "Front2", "Back");
            Helper.CreateFlashcardText(userId, "Custom", "Front3", "Back");

            GoToQuiz("Custom");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            IWebElement front = driver.FindElement(By.ClassName("front-text"));
            Assert.IsTrue(front.Text.Contains("Front1"));
            var veryEasyBtn = driver.FindElement(By.Id("MainContent_very_easy_btn"));
            veryEasyBtn.Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            front = driver.FindElement(By.ClassName("front-text"));
            Assert.IsTrue(front.Text.Contains("Front2"));
            veryEasyBtn = driver.FindElement(By.Id("MainContent_very_easy_btn"));
            veryEasyBtn.Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            front = driver.FindElement(By.ClassName("front-text"));
            Assert.IsTrue(front.Text.Contains("Front3"));
            veryEasyBtn = driver.FindElement(By.Id("MainContent_very_easy_btn"));
            veryEasyBtn.Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            front = driver.FindElement(By.ClassName("front-text"));
            Assert.IsTrue(front.Text.Contains("Front1"));

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
