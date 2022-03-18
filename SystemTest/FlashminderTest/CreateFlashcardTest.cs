using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Events;
using System.Configuration;
using NUnit.Framework;

namespace FlashminderTest
{
    class CreateFlashcardTest
    {
        string url = "";
        string imagePath = "";
        WebDriver driver;
        int userID;

        [SetUp]
        public void StartBrowser()
        {
            url = ConfigurationManager.AppSettings.Get("Website");
            imagePath = ConfigurationManager.AppSettings.Get("ImagesPath");
            driver = new ChromeDriver("./");
            userID = Helper.CreateUser("Test1", "Test");
        }

        public bool GoToCreateFlashcard()
        {
            Helper.SignIn(driver, url, "Test1", "Test");
            driver.Navigate().GoToUrl(url + "/CreateFlashcard.aspx");
            return driver.Title == "Create Flashcard";
        }

        public bool CreateFlashcardWithText(string category, string front, string back)
        {
            bool success = false;
            if (driver != null)
            {
                GoToCreateFlashcard();
                var categoryDropdown = driver.FindElement(By.Id("MainContent_category_dropdownlist"));
                var frontText = driver.FindElement(By.Id("MainContent_front_txtbx"));
                var backText = driver.FindElement(By.Id("MainContent_back_txtbx"));

                var createButton = driver.FindElement(By.Id("MainContent_create_btn"));

                frontText.SendKeys(front);
                backText.SendKeys(back);
                if (!string.IsNullOrEmpty(category))
                {
                    categoryDropdown.SendKeys(category);
                }
                createButton.Click();

                var message = driver.FindElement(By.Id("MainContent_message_lbl"));

                if (message.Text.Contains("Success"))
                {
                    success = true;
                }
            }
            return success;
        }

        public bool CreateFlashcardWithImage(string category, string frontImagePath, string backImagePath)
        {
            bool success = false;
            if (driver != null)
            {
                GoToCreateFlashcard();
                var categoryDropdown = driver.FindElement(By.Id("MainContent_category_dropdownlist"));
                var frontUpload = driver.FindElement(By.Id("MainContent_front_upload"));
                var backUpload = driver.FindElement(By.Id("MainContent_back_upload"));
                var createButton = driver.FindElement(By.Id("MainContent_create_btn"));

                if (!string.IsNullOrEmpty(category))
                {
                    categoryDropdown.SendKeys(category);
                }
                if (!string.IsNullOrEmpty(frontImagePath))
                {
                    frontUpload.SendKeys(frontImagePath);
                }
                if (!string.IsNullOrEmpty(backImagePath))
                {
                    backUpload.SendKeys(backImagePath);
                }

                createButton.Click();

                var message = driver.FindElement(By.Id("MainContent_message_lbl"));

                if (message.Text.Contains("Success"))
                {
                    success = true;
                }
            }
            return success;
        }

        public bool CreateFlashcardWithMixed(string category, string front, string back, string frontImagePath, string backImagePath)
        {
            bool success = false;
            if (driver != null)
            {
                GoToCreateFlashcard();
                var categoryDropdown = driver.FindElement(By.Id("MainContent_category_dropdownlist"));
                var frontUpload = driver.FindElement(By.Id("MainContent_front_upload"));
                var frontText = driver.FindElement(By.Id("MainContent_front_txtbx"));
                var backUpload = driver.FindElement(By.Id("MainContent_back_upload"));
                var backText = driver.FindElement(By.Id("MainContent_back_txtbx"));

                var createButton = driver.FindElement(By.Id("MainContent_create_btn"));

                if (!string.IsNullOrEmpty(category))
                {
                    categoryDropdown.SendKeys(category);
                }
                frontText.SendKeys(front);
                if (!string.IsNullOrEmpty(frontImagePath))
                {
                    frontUpload.SendKeys(frontImagePath);
                }
                backText.SendKeys(back);
                if (!string.IsNullOrEmpty(backImagePath))
                {
                    backUpload.SendKeys(backImagePath);
                }

                createButton.Click();

                var message = driver.FindElement(By.Id("MainContent_message_lbl"));

                if (message.Text.Contains("Success"))
                {
                    success = true;
                }
            }
            return success;
        }

        [Test]
        public void TestTextInput()
        {
            Assert.IsTrue(CreateFlashcardWithText("", "Front", "Back"));
        }

        [Test]
        public void TestImageInput()
        {
            Assert.IsTrue(CreateFlashcardWithImage("", imagePath+"\\front_test.png", imagePath + "\\back_test.png"));
        }

        [Test]
        public void TestTextFrontImageBack()
        {
            Assert.IsTrue(CreateFlashcardWithMixed("", "Test", "", "", imagePath + "\\back_test.png"));
        }

        [Test]
        public void TestImageFrontTextBack()
        {
            Assert.IsTrue(CreateFlashcardWithMixed("", "", "Test", imagePath + "\\back_test.png", ""));
        }

        public bool CreateCategory(string category, string categoryDesc)
        {
            bool success = false;
            GoToCreateFlashcard();
            string mainHandle = driver.CurrentWindowHandle;
            var createCategoryBtn = driver.FindElement(By.Id("createCategory_btn"));
            createCategoryBtn.Click();
            foreach(string handle in driver.WindowHandles)
            {
                if(handle != mainHandle)
                {
                    driver.SwitchTo().Window(handle);
                }
            }

            var categoryName = driver.FindElement(By.Id("categoryName_txtbx"));
            var categoryDescription = driver.FindElement(By.Id("categoryDesc_txtbx"));
            var createCategoryButton = driver.FindElement(By.Id("create_btn"));
            categoryName.SendKeys(category);
            categoryDescription.SendKeys(categoryDesc);
            createCategoryButton.Click();

            var message = driver.FindElement(By.Id("message_lbl"));

            if (message.Text.Contains("Success"))
            {
                success = true;
            }

            driver.SwitchTo().Window(mainHandle);

            return success;
        }

        [Test]
        public void CreateCategoryWithDescription()
        {
            Assert.IsTrue(CreateCategory("Category1", "Description"));
        }

        [Test]
        public void CreateExistingCategory()
        {
            Helper.CreateCategory(userID, "CategoryRepeat", "");
            Assert.IsFalse(CreateCategory("CategoryRepeat", "Description"));
        }

        [Test]
        public void CreateCategoryWithoutDescription()
        {
            Assert.IsTrue(CreateCategory("Category2", ""));
        }

        [TearDown]
        public void CloseBrowser()
        {
            if (driver != null)
            {
                driver.Close();
                driver.Quit();
            }
            Helper.RemoveUser(userID);
        }
    }
}
