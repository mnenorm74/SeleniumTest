using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTest
{
    public class Task5
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private const string loginPage = "http://localhost/litecart/admin/login.php";
        
        [SetUp]
        public void Setup()
        {
            var options = new FirefoxOptions {UseLegacyImplementation = true};
            driver = new FirefoxDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        
        [Test]
        // Проверка заголовка страницы авторизации
        public void PageTitleIsMyStore()
        {
            driver.Url = loginPage;
            wait.Until(ExpectedConditions.TitleIs("My Store"));
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
            driver = null;
        }
    }
}