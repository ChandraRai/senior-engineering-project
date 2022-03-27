using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Configuration;
using NUnit.Framework;

using Flashminder.Models;

namespace FlashminderTest
{
    class SignupTest
    {
        string url = "";
        WebDriver driver;
        [SetUp]
        public void StartBrowser()
        {
            url = ConfigurationManager.AppSettings.Get("Website");
            driver = new ChromeDriver("./");
        }
        public bool SignupRegister(string usernameIn, string emailIn, string passwordIn)
        {
            bool success = false;
            if (!string.IsNullOrEmpty(url))
            {
                driver.Navigate().GoToUrl(url + "/Signup.aspx");
                var username = driver.FindElement(By.Id("MainContent_txtUsername"));
                var email = driver.FindElement(By.Id("MainContent_txtEmail"));
                var password = driver.FindElement(By.Id("MainContent_txtPassword"));
                var passwordConfirm = driver.FindElement(By.Id("MainContent_txtConfirm"));
                var registerButton = driver.FindElement(By.Id("MainContent_RegisterButton"));

                username.SendKeys(usernameIn);
                email.SendKeys(emailIn);
                password.Click();
                password.Clear();
                password.SendKeys(passwordIn);
                passwordConfirm.Click();
                passwordConfirm.Clear();
                passwordConfirm.SendKeys(passwordIn);
                registerButton.Click();
                if (driver.Title == "Sign in")
                {
                    success = true;
                }
            }
            return success;
        }

        [Test]
        public void SignupSuccessful()
        {
            Assert.IsTrue(SignupRegister("Test1", "abc@test.com", "password"));
        }

        [Test]
        public void SignupExistingUsername()
        {
            Helper.CreateUser("Test1", "password");
            Assert.IsFalse(SignupRegister("Test1", "abc@test.com", "password"));
        }

        [Test]
        public void SignupSuccessfulDatabase()
        {
            SignupRegister("Test2", "abc@test.com", "password");
            USERS user = null;
            using (DefaultConnection db = new DefaultConnection())
            {
                user = db.USERS.Where(user => (user.Username == "Test2")).FirstOrDefault();
            }
            Assert.IsTrue(user.Email == "abc@test.com");
        }

        [Test]
        public void SignupNoUsername()
        {
            bool success = false;
            if (!string.IsNullOrEmpty(url))
            {
                driver.Navigate().GoToUrl(url + "/Signup.aspx");
                var email = driver.FindElement(By.Id("MainContent_txtEmail"));
                var password = driver.FindElement(By.Id("MainContent_txtPassword"));
                var passwordConfirm = driver.FindElement(By.Id("MainContent_txtConfirm"));
                var registerButton = driver.FindElement(By.Id("MainContent_RegisterButton"));


                email.SendKeys("abc@test.com");
                password.SendKeys("test");
                passwordConfirm.SendKeys("test");
                registerButton.Click();
                if (driver.FindElement(By.Id("MainContent_RequiredFieldValidator1")) != null)
                {
                    success = true;
                }
            }
            Assert.IsTrue(success);
        }

        [Test]
        public void SignupNoEmail()
        {
            bool success = false;
            if (!string.IsNullOrEmpty(url))
            {
                driver.Navigate().GoToUrl(url + "/Signup.aspx");
                var username = driver.FindElement(By.Id("MainContent_txtUsername"));
                var email = driver.FindElement(By.Id("MainContent_txtEmail"));
                var password = driver.FindElement(By.Id("MainContent_txtPassword"));
                var passwordConfirm = driver.FindElement(By.Id("MainContent_txtConfirm"));
                var registerButton = driver.FindElement(By.Id("MainContent_RegisterButton"));

                username.SendKeys("Test1");
                email.SendKeys("abc@test.com");
                password.SendKeys("test");
                passwordConfirm.SendKeys("test");
                registerButton.Click();
                if (driver.FindElement(By.Id("MainContent_RequiredFieldValidator2")) != null)
                {
                    success = true;
                }
            }
            Assert.IsTrue(success);
        }

        public void SignupNoPassword()
        {
            bool success = false;
            if (!string.IsNullOrEmpty(url))
            {
                driver.Navigate().GoToUrl(url + "/Signup.aspx");
                var username = driver.FindElement(By.Id("MainContent_txtUsername"));
                var email = driver.FindElement(By.Id("MainContent_txtEmail"));
                var passwordConfirm = driver.FindElement(By.Id("MainContent_txtConfirm"));
                var registerButton = driver.FindElement(By.Id("MainContent_RegisterButton"));

                username.SendKeys("Test1");
                email.SendKeys("abc@test.com");
                passwordConfirm.SendKeys("test");
                registerButton.Click();
                if (driver.FindElement(By.Id("MainContent_RequiredFieldValidator3")) != null)
                {
                    success = true;
                }
            }
            Assert.IsTrue(success);
        }

        [Test]
        public void SignupNoPasswordConfirm()
        {
            bool success = false;
            if (!string.IsNullOrEmpty(url))
            {
                driver.Navigate().GoToUrl(url + "/Signup.aspx");
                var username = driver.FindElement(By.Id("MainContent_txtUsername"));
                var email = driver.FindElement(By.Id("MainContent_txtEmail"));
                var password = driver.FindElement(By.Id("MainContent_txtPassword"));
                var registerButton = driver.FindElement(By.Id("MainContent_RegisterButton"));

                username.SendKeys("Test1");
                email.SendKeys("abc@test.com");
                password.SendKeys("test");
                registerButton.Click();
                if (driver.FindElement(By.Id("MainContent_RequiredFieldValidator4")) != null)
                {
                    success = true;
                }
            }
            Assert.IsTrue(success);
        }

        [Test]
        public void SignupTwoDifferentPasswords()
        {
            bool success = false;
            if (!string.IsNullOrEmpty(url))
            {
                driver.Navigate().GoToUrl(url + "/Signup.aspx");
                var username = driver.FindElement(By.Id("MainContent_txtUsername"));
                var email = driver.FindElement(By.Id("MainContent_txtEmail"));
                var password = driver.FindElement(By.Id("MainContent_txtPassword"));
                var passwordConfirm = driver.FindElement(By.Id("MainContent_txtConfirm"));
                var registerButton = driver.FindElement(By.Id("MainContent_RegisterButton"));

                username.SendKeys("Test1");
                email.SendKeys("abc@test.com");
                password.SendKeys("test");
                passwordConfirm.SendKeys("testtsdts");
                registerButton.Click();
            }
            Assert.IsTrue(!success);
        }

        [TearDown]
        public void CloseBrowser()
        {
            if (driver != null)
            {
                driver.Close();
                driver.Quit();
            }

            using (DefaultConnection db = new DefaultConnection())
            {
                foreach(USERS user in db.USERS.Where(user => (user.Username == "Test1")))
                {
                    db.USERS.Remove(user);
                }
                foreach (USERS user in db.USERS.Where(user => (user.Username == "Test2")))
                {
                    db.USERS.Remove(user);
                }
                db.SaveChanges();
            }

        }

    }
}
