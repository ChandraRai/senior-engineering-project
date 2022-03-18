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
    class LogoutTest
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

        [Test]
        public void LogoutSuccess()
        {
            Helper.SignIn(driver, url, "Test1", "Test");
            var logout = driver.FindElement(By.Id("ctl08_logout"));
            logout.Click();
            Assert.IsTrue(driver.Title == "About");
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
