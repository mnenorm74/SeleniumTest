using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTest
{
    public class Task7
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private const string adminPage = "http://localhost/litecart/admin/";
        
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Login();
        }

        private void Login()
        {
            driver.Url = $"{adminPage}login.php";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.ClassName("btn")).Click();
            wait.Until(ExpectedConditions.UrlToBe(adminPage));
        }
        
        // Проверка наличия заголовков при переходе по пунктам и подпунктам меню
        [Test]
        public void PagesContainHeader()
        {
            var menuItemsCount = driver.FindElements(By.ClassName("app")).Count;
            for (var i = 1; i < menuItemsCount + 1; i++)
            {
                CheckPageHeader("app", i, "panel-heading");
                var subItemsCount = driver.FindElements(By.ClassName("doc")).Count;
                for (var j = 1; j < subItemsCount + 1; j++)
                {
                    CheckPageHeader("doc", j, "panel-heading");
                }
            }
        }

        private void CheckPageHeader(string itemClassName, int index, string headerClassName)
        {
            driver.FindElement(By.CssSelector($".{itemClassName}:nth-child({index})")).Click();
            Assert.AreEqual(HasElement(By.ClassName(headerClassName)), true);
        }
        
        private bool HasElement(By selector)
        {
            return driver.FindElements(selector).Count > 0;
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
            driver = null;
        }
    }
}