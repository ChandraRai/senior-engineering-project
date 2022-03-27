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
    class SignInTest
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

        public bool SignIn(string username, string password)
        {
            bool success = false;
            if (!string.IsNullOrEmpty(url))
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
        [Test]
        public void SuccessfulSignIn()
        {
            Assert.IsTrue(SignIn("Test1", "Test")); 
        }

        [Test]
        public void FailSignIn_BadPassword()
        {
            Assert.IsFalse(SignIn("Test1", "password"));
        }

        [Test]
        public void FailSignIn_BadUsername()
        {
            Assert.IsFalse(SignIn("Test2", "Test"));
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
